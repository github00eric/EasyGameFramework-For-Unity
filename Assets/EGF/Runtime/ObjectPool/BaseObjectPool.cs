using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// EGF基础对象池，仿Unity对象池风格
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseObjectPool<T>: IObjectPool<T>
    {
        private readonly Queue<T> pool;
        private int countActive;
        private float lastGetElementTime;
        
        private readonly Func<T> createFunc;
        private readonly Action<T> actionOnGet;
        private readonly Action<T> actionOnRelease;
        private readonly Action<T> actionOnDestroy;
        private readonly bool collectionCheck;
        private readonly int defaultSize;
        private readonly int maxSize;

        public int CountInactive => pool.Count;
        public int CountActive => countActive;
        public int DefaultSize => defaultSize;

        /// <summary>
        /// EGF基础对象池，仿Unity对象池风格
        /// </summary>
        /// <param name="createFunc">创建对象委托</param>
        /// <param name="actionOnGet">从对象池取出委托</param>
        /// <param name="actionOnRelease">放回对象池委托</param>
        /// <param name="actionOnDestroy">销毁对象委托</param>
        /// <param name="collectionCheck"></param>
        /// <param name="defaultSize">默认容量（持续稳定使用时的对象总数量）</param>
        /// <param name="maxSize">最大容量</param>
        public BaseObjectPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRelease, Action<T> actionOnDestroy, bool collectionCheck = true, int defaultSize = 10, int maxSize = 10000)
        {
            this.createFunc = createFunc;
            this.actionOnGet = actionOnGet;
            this.actionOnRelease = actionOnRelease;
            this.actionOnDestroy = actionOnDestroy;
            this.collectionCheck = collectionCheck;
            this.defaultSize = defaultSize;
            this.maxSize = maxSize;

            lastGetElementTime = Time.realtimeSinceStartup;
            pool = new Queue<T>();
        }


        public void Clear()
        {
            lock (pool)
            {
                int count = pool.Count;
                for (int i = 0; i < count; i++)
                {
                    var temp = pool.Dequeue();
                    if(temp!=null)
                        actionOnDestroy(temp);
                }
            }
        }

        public T Get()
        {
            var result = TakeFromPool();
            actionOnGet?.Invoke(result);
            countActive++;
            lastGetElementTime = Time.realtimeSinceStartup;
            return result;
        }

        public void Release(T element)
        {
            if (element == null) return;
            ReturnToPool(element);
            countActive--;
        }
        
        private T TakeFromPool()
        {
            if (pool.Count <= 0)
            {
                var temp = createFunc.Invoke();
                return temp;
            }
            
            var result = pool.Dequeue();
            return result;
        }

        private void ReturnToPool(T element)
        {
#if UNITY_EDITOR
            if (collectionCheck)
            {
                if (pool.Contains(element))
                {
                    Logcat.Warning(this,"[BaseObjectPool] 错误操作：试图将已经归还的对象放回对象池。");
                    return;
                }
            }
#endif
            if (pool.Count < maxSize)
            {
                actionOnRelease?.Invoke(element);
                pool.Enqueue(element);
            }
            else
            {
                // 超最大容量的对象直接销毁
                actionOnDestroy?.Invoke(element);
            }
        }
        
        public float LastGetElementTime => lastGetElementTime;
        public void OptimizeCreateElement(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var temp = createFunc.Invoke();
                pool.Enqueue(temp);
            }
        }

        public void OptimizeDestroyElement(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var temp = pool.Dequeue();
                if(temp!=null)
                    actionOnDestroy?.Invoke(temp);
            }
        }
    }
}
