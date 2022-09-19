using UnityEngine;

namespace EGF.Runtime
{
    public interface IUIManager
    {
        /// <summary>
        /// 获取和显示新UI，新UI显示在其它UI上方，并暂停其它UI的交互
        /// </summary>
        UiView ShowFocus(string panelName);     // --> ShowMask(string panelName)

        /// <summary>
        /// 获取和显示新UI，新UI显示在其它UI上方
        /// </summary>
        UiView Show(string panelName);
        
        // 显示 UI，新 UI 显示在上方，不暂停其它 UI 交互
        // UIView ShowAdd(string panelName);
        
        // 显示 悬浮UI，悬浮UI 可以拖拽，且不会因下层 UI 关闭而关闭
        // UIView ShowFloat(string panelName);
        
        // 显示 Toast弹窗，该显示方式无法交互，会在指定时间后自动关闭
        // UIView ShowToast(string panelName, float duration);

        /// <summary>
        /// 不显示栈顶UI时调用
        /// </summary>
        UiView Hide();

        /// <summary>
        /// 查看当前栈顶UI
        /// </summary>
        UiView Peek();
    }
}
