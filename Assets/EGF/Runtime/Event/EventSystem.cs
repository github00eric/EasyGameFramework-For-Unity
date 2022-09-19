using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EGF.Runtime
{
    public class EventSystem : MonoSingleton<EventSystem>,IEventSystem
    {
        [HideLabel,ReadOnly,Multiline(4)]
        public string describe = "事件系统，提供模块接口 > IEventSystem" +
                                 "\n使用字符串管理的自定义事件系统，可方便地将代码函数注册为事件，并通过一串字符索引调用";

        private bool _initialized;
        
        [Title("Event List")]
        [ShowInInspector]
        private readonly Dictionary<string, Delegate> _events
            = new Dictionary<string, Delegate>();

        protected override void Initialization()
        {
            this._initialized = true;
            EgfEntry.RegisterModule<IEventSystem>(this);
        }

        protected override void Release()
        {
            EgfEntry.UnRegisterModule<IEventSystem>();
            _events.Clear();
            this._initialized = false;
        }

        #region IEventSystem
        
        public void AddEventListener(string eventName, EventAction onEventTrigger)
        {
            if(!_initialized) return;
            OnListenerAdding(eventName,onEventTrigger);
            _events[eventName] = _events[eventName] as EventAction + onEventTrigger;
        }

        public void AddEventListener<T>(string eventName, EventAction<T> onEventTrigger) where T : IEventArgs
        {
            if(!_initialized) return;
            OnListenerAdding(eventName,onEventTrigger);
            _events[eventName] = _events[eventName] as EventAction<T> + onEventTrigger;
        }

        public void RemoveEventListener(string eventName, EventAction onEventTrigger)
        {
            if(!_initialized) return;
            OnListenerRemoving(eventName,onEventTrigger);
            _events[eventName] = _events[eventName] as EventAction - onEventTrigger;
            OnListenerRemoved(eventName);
        }

        public void RemoveEventListener<T>(string eventName, EventAction<T> onEventTrigger) where T : IEventArgs
        {
            if(!_initialized) return;
            OnListenerRemoving(eventName,onEventTrigger);
            _events[eventName] = _events[eventName] as EventAction<T> - onEventTrigger;
            OnListenerRemoved(eventName);
        }

        public void EventTrigger(string eventName)
        {
            if(!_initialized) return;
            OnEventTrigger(eventName);
            if (_events.TryGetValue(eventName, out Delegate temp))
            {
                if (temp is EventAction act)
                {
                    act();
                }
                else
                {
                    Logcat.Info(this,$"触发 {eventName} 事件类型不符。");
                }
            }
        }

        public void EventTrigger<T>(string eventName, T args) where T : IEventArgs
        {
            if(!_initialized) return;
            OnEventTrigger(eventName);
            if (_events.TryGetValue(eventName, out Delegate temp))
            {
                if (temp is EventAction<T> act)
                {
                    act(args);
                }
                else
                {
                    Logcat.Info(this,$"触发 {eventName} 事件类型不符。");
                }
            }
        }


        #endregion
        
        #region 内部函数

        private void OnListenerAdding(string eventName, EventAction listener)
        {
            if (!_events.ContainsKey(eventName)) {
                _events.Add(eventName, null );
                Logcat.Info(this,$"添加 {eventName} 事件，类型为{listener.GetType().Name}");
            }

            if (_events[eventName] != null && !(_events[eventName] is EventAction))
            {
                Logcat.Warning(this,$"新添加的 {eventName} 事件与已有事件类型不同。已有事件类型为 {_events[eventName].GetType().Name}，新事件类型为 {listener.GetType().Name}");
            }
        }
        
        private void OnListenerAdding<T>(string eventName, EventAction<T> listener) where T: IEventArgs
        {
            if (!_events.ContainsKey(eventName)) {
                _events.Add(eventName, null );
                Logcat.Info(this,$"添加 {eventName} 事件，类型为{listener.GetType().Name}");
            }

            if (_events[eventName] != null && !(_events[eventName] is EventAction<T>))
            {
                Logcat.Warning(this,$"新添加的 {eventName} 事件与已有事件类型不同。已有事件类型为 {_events[eventName].GetType().Name}，新事件类型为 {listener.GetType().Name}");
            }
        }

        private void OnListenerRemoving(string eventName, EventAction listener)
        {
            Logcat.Info(this,$"移除 {eventName} 事件，待移除类型为{listener.GetType().Name}");
            if (_events.ContainsKey(eventName))
            {
                if (_events[eventName] == null)
                {
                    Logcat.Warning(this,$"已有事件 {eventName} 类型为 null，移除失败。");
                }
                else if (!(_events[eventName] is EventAction))
                {
                    Logcat.Warning(this,$"事件 {eventName} 类型与已有事件不符，已有事件类型为 {_events[eventName].GetType().Name}。");
                }
            }
            else
            {
                Logcat.Warning(this,$"事件 {eventName} 不存在，移除失败。");
            }
        }
        
        private void OnListenerRemoving<T>(string eventName, EventAction<T> listener) where T: IEventArgs
        {
            Logcat.Info(this,$"移除 {eventName} 事件，待移除类型为{listener.GetType().Name}");
            if (_events.ContainsKey(eventName))
            {
                if (_events[eventName] == null)
                {
                    Logcat.Warning(this,$"已有事件 {eventName} 类型为 null，移除失败。");
                }
                else if (!(_events[eventName] is EventAction<T>))
                {
                    Logcat.Warning(this,$"事件 {eventName} 类型与已有事件不符，已有事件类型为 {_events[eventName].GetType().Name}。");
                }
            }
            else
            {
                Logcat.Warning(this,$"事件 {eventName} 不存在，移除失败。");
            }
        }

        private void OnListenerRemoved(string eventName)
        {
            if (_events.ContainsKey(eventName) && _events[eventName] == null)
            {
                _events.Remove(eventName);
            }
        }

        private void OnEventTrigger(string eventName)
        {
            Logcat.Info(this,$" {eventName} 事件触发。");
            if (!_events.ContainsKey(eventName))
            {
                Logcat.Warning(this,$" {eventName} 事件调用失败，事件不存在。");
            }
        }

        #endregion
    }
}
