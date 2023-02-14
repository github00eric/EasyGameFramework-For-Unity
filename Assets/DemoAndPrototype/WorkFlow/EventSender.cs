using System;
using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using NaughtyAttributes;
using UnityEngine;


public class EventSender : MonoBehaviour
{
    private IEventSystem eventSystem;
    
    void Start()
    {
        eventSystem = EgfEntry.GetModule<IEventSystem>();
    }

    /// <summary>
    /// 事件调用测试
    /// </summary>
    [Button]
    private void AskToDoSomething()
    {
        // 注册 Do Something 事件后，可在任意时刻任意位置触发
        eventSystem.EventTrigger("Do Something");
    }

    public string message;
    
    // 定义数据内容，只要继承 IEventArgs 接口均可作为数据包
    public struct SomeData: IEventArgs
    {
        public string Message;
    }
    
    /// <summary>
    /// 发送数据测试
    /// </summary>
    [Button]
    private void SendMessage()
    {
        string tempMsg = string.IsNullOrEmpty(message) ? "This is a secret Message. " : message;
        
        SomeData data = new SomeData()
        {
            Message = tempMsg
        };
        eventSystem.EventTrigger("Send Data",data);
    }
}
