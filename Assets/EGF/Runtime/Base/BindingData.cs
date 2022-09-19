using System;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// 绑定型数据
    /// <para>可向该数据加入更新事件，外部更新数据值则自动调用更新事件，用于分离控制和表现</para>
    /// <para>如果是引用型数据修改自身，不会触发更新事件</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class BindingData<T> where T: IEquatable<T>
    {
        [SerializeField] private T dataValue = default;
        private event Action<T> OnValueChanged;

        public T Value
        {
            get => dataValue;
            set
            {
                if (value.Equals(dataValue))
                {
                    return;
                }
                dataValue = value;
                OnValueChanged?.Invoke(dataValue);
            }
        }

        public void AddValueChangedEventListener(Action<T> onValueChangedEvent)
        {
            if (onValueChangedEvent == null) return;
            
            onValueChangedEvent.Invoke(dataValue);
            this.OnValueChanged += onValueChangedEvent;
        }

        public void RemoveValueChangedEventListener(Action<T> onValueChangedEvent)
        {
            this.OnValueChanged -= onValueChangedEvent;
        }
    }
}
