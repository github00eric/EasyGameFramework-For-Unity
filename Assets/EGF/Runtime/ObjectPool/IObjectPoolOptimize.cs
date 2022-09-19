namespace EGF.Runtime
{
    public interface IObjectPoolOptimizer
    {
        /// <summary>
        /// 对象池使用压力评估参数 P，使用压力越大，池中需要准备越多对象
        /// </summary>
        float PressureUsing { get; }

        /// <summary>
        /// 设置优化操作执行频率
        /// <para>默认为 xxx </para>
        /// </summary>
        /// <param name="rate"></param>
        void SetOptimizeRate(float rate);

        /// <summary>
        /// 持续调用，按设定频率定期执行优化算法
        /// </summary>
        void Update();
    }
}