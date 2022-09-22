using System;
using UnityEngine;

namespace EGF.Runtime
{
    public interface IUIManager
    {
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

    }
}
