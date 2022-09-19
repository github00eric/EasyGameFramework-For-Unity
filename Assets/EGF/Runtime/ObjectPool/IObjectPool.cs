using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// 模仿Unity风格的对象池接口
    /// <para>可以在 2021.2+ 版本的Unity中方便地转换为官方实现对象池</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPool<T>
    {
        /// <summary>
        /// 对象池当前剩余的对象总数
        /// </summary>
        int CountInactive { get; }

        /// <summary>
        /// 对象池激活对象总数
        /// </summary>
        int CountActive { get; }
        
        /// <summary>
        /// 对象池默认容量
        /// </summary>
        int DefaultSize { get; }
        
        /// <summary>
        /// 对象池最后一次取出对象的时间
        /// </summary>
        float LastGetElementTime { get; }
        
        /// <summary>
        /// 清空对象池
        /// </summary>
        void Clear();
        
        /// <summary>
        /// 从对象池取出
        /// </summary>
        /// <returns></returns>
        T Get();
        
        /// <summary>
        /// 将对象实例归还到对象池
        /// </summary>
        /// <param name="element"></param>
        void Release(T element);

        /// <summary>
        /// 供优化器使用，主动创建指定数目的对象池元素
        /// </summary>
        /// <param name="count"></param>
        void OptimizeCreateElement(int count);

        /// <summary>
        /// 供优化器使用，主动删除指定数目的对象池元素
        /// </summary>
        /// <param name="count"></param>
        void OptimizeDestroyElement(int count);

    }
    
}
