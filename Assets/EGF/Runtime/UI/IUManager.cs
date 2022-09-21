using System;
using UnityEngine;

namespace EGF.Runtime
{
    public interface IUIManager
    {
        /// <summary>
        /// 获取和显示新UI，新UI显示在其它UI上方，并暂停其它UI的交互
        /// </summary>
        [Obsolete("ShowFocus is Obsolete, Use ShowAdditive instead")]
        UiView ShowFocus(string panelName);     // --> ShowMask(string panelName)
        // ----------------------------------------------------------------------------------------------------
        
        Camera GetUiCamera();
        
        /// <summary>
        /// 获得一个界面实例但此时不展示界面，可进一步初始化
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        UiView GetUi(string panelName);
        
        /// <summary>
        /// 查看当前栈顶 普通UI
        /// </summary>
        UiView Peek();

        /// <summary>
        /// 获取和显示新UI，关闭旧 UI
        /// </summary>
        UiView Show(string panelName);
        UiView Show(UiView uiInstance);
        
        /// <summary>
        /// 显示 UI，新 UI 显示在上方，暂停其它 UI 交互
        /// </summary>
        UiView ShowAdditive(string panelName);
        UiView ShowAdditive(UiView uiInstance);
        
        // 显示 悬浮UI，悬浮UI 可以拖拽，且不会因下层 UI 关闭而关闭
        // UIView ShowFloat(string panelName);
        
        // 显示 Toast弹窗，该显示方式无法交互，会在指定时间后自动关闭
        // UIView ShowToast(string panelName, float duration);

        /// <summary>
        /// 关闭栈顶 UI
        /// </summary>
        UiView Hide();

        /// <summary>
        /// 普通界面：
        /// <para>关闭 panelName 界面以及在该界面上层的其它界面</para>
        /// <para></para>
        /// 悬浮界面：
        /// <para>关闭 panelName 界面</para>
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        UiView Hide(string panelName);

    }
}
