using System;
using EGF.Runtime;

namespace EGF.Runtime
{
    public interface IEventArgs{}
    public delegate void EventAction();
    public delegate void EventAction<T>(T args) where T: IEventArgs;
    public interface IEventSystem
    {
        /// <summary>
        /// 开始监听事件
        /// </summary>
        /// <param name="eventName">监听的事件名</param>
        /// <param name="onEventTrigger">事件发生时执行的动作</param>
        void AddEventListener(string eventName, EventAction onEventTrigger);

        void AddEventListener<T>(string eventName, EventAction<T> onEventTrigger) where T : IEventArgs;
        
        /// <summary>
        /// 取消监听事件
        /// </summary>
        /// <param name="eventName">监听的事件名</param>
        /// <param name="onEventTrigger">事件发生时取消执行</param>
        void RemoveEventListener(string eventName, EventAction onEventTrigger);

        void RemoveEventListener<T>(string eventName, EventAction<T> onEventTrigger) where T : IEventArgs;
        
        /// <summary>
        /// 立即触发事件
        /// </summary>
        /// <param name="eventName"></param>
        void EventTrigger(string eventName);

        void EventTrigger<T>(string eventName, T args) where T : IEventArgs;
    }
}
