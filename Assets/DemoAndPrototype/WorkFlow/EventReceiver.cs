using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    private IEventSystem eventSystem;
    
    void Start()
    {
        eventSystem = EgfEntry.GetModule<IEventSystem>();
        
        eventSystem.AddEventListener("Do Something",LogTest);    // 注册 Do Something 事件，Do Something 触发时调用 LogTest 方法
        eventSystem.AddEventListener<EventSender.SomeData>("Send Data",OnGetData);    // 注册 Send Data 事件接收 SomeData 数据，触发时调用 OnGetData 方法
    }
    
    private void OnDestroy()
    {
        // 注册和使用完毕后记得释放
        eventSystem.AddEventListener("Do Something",LogTest);
        eventSystem.AddEventListener<EventSender.SomeData>("Send Data",OnGetData);
    }

    [ReadOnly]
    public string receivedData;
    
    private void OnGetData(EventSender.SomeData data)
    {
        Debug.Log($"I get data: {data.Message}.");
        receivedData = data.Message;
    }
    
    private void LogTest()
    {
        Debug.Log("Get Event...........I should do something. ");
    }
    
    /// <summary>
    /// 错误反注册测试
    /// </summary>
    [Button]
    private void ErrorTest1()
    {
        eventSystem.RemoveEventListener("Send Data",LogTest);
        eventSystem.RemoveEventListener<EventSender.SomeData>("Do Something",OnGetData);
        // eventSystem.UnRegisterEvent("Send Data",OnGetData);
    }
}
