using EGF.Runtime;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// EGF对象池自动优化器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPoolOptimizer<T>: IObjectPoolOptimizer
    {
        private const float DefaultOptimizeRate = 30;    // 默认优化频率
        private const float MinPressure = 0.2f;    // 最小容忍使用压力
        private const float MaxPressure = 0.4f;    // 最大容忍使用压力
        private const float ChangeStep = 0.2f;     // 每次优化改动对象数量占总容量比率 20%
    
        private readonly IObjectPool<T> pool;
        private float timer;
        private float optimizeRate;    // 实际执行优化算法频率

        /// <summary>
        /// EGF对象池自动优化器
        /// </summary>
        /// <param name="pool">待优化的对象池</param>
        public ObjectPoolOptimizer(IObjectPool<T> pool)
        {
            this.pool = pool;
            timer = Random.Range(0,DefaultOptimizeRate);
            optimizeRate = DefaultOptimizeRate;
        }

        public float PressureUsing
        {
            // 根据 对象池默认容量 Ds、激活中的对象数量 Na、未激活的对象数量 Nd 来计算使用压力
            // 理想情况下：
            // 使用压力最小时：Na = 0, Nd = Ds, P = (Na-Nd)/Ds = -1，对象池内对象已有足够准备无需创建，立即可用
            // 使用压力最大时：Na = Ds, Nd = 0, P = (Na-Nd)/Ds = 1，无空余对象，随时需要创建新对象
            // 刚开始使用对象池：Na = 0, Nd = 0, P = 0, 压力值居中，使用时需要创建对象。
            // P 的范围从 [-1,1] 归一化为 [0,1] 后为Pn：Pn = (P + 1)/2 = (Na - Nd + Ds) / (2 * Ds)
            //
            // 最终
            // 压力参数 Pn = (Na - Nd + Ds) / (2 * Ds)
            // 正常范围 [0,1] （实际可大于1）
            // 
            // 该算法下，
            // 对象池使用速率越快压力越大，满载动态平衡后，压力接近1
            // 对象池暂未使用，且对象准备充足（创建对象数达默认容量），压力为0
            // 无对象，暂未开始使用（储备不足，一旦开始使用，需创建对象），压力为0.5
            get
            {
                float result = pool.CountActive - pool.CountInactive + pool.DefaultSize;
                result = result / (pool.DefaultSize * 2);
                return result;
            }
        }

        public void SetOptimizeRate(float rate)
        {
            optimizeRate = rate > 0 ? rate : DefaultOptimizeRate;
        }

        public void Update()
        {
            if (timer < optimizeRate)
            {
                timer += Time.deltaTime;
                return;
            }
            timer = 0;
            
            Optimize();
        }

        private void Optimize()
        {
            Logcat.Info(this,"Optimize Once");
            
            // 如果最近在使用对象池，就不去优化了
            float temp = Time.realtimeSinceStartup - pool.LastGetElementTime;
            if (temp < optimizeRate) return;
            
            if (PressureUsing > MaxPressure)
            {
                var changeCount = (int)(pool.DefaultSize * ChangeStep);
                pool.OptimizeCreateElement(changeCount);
            }
            else if (PressureUsing < MinPressure)
            {
                var changeCount = (int)(pool.DefaultSize * ChangeStep);
                pool.OptimizeDestroyElement(changeCount);
            }
        }
    }
}