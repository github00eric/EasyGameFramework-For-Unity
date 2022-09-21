/* Created by Eric-C.
 * 基本的UI页面脚本
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// UI页面基类，
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UiView : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null)
                    canvasGroup = GetComponent<CanvasGroup>();
                return canvasGroup;
            }
        }
        
        /// <summary>
        /// 显示UI
        /// </summary>
        internal void ShowCanvasGroup()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
        }
        
        /// <summary>
        /// 隐藏UI
        /// </summary>
        internal void HideCanvasGroup()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 显示和激活当前Panel交互
        /// </summary>
        public virtual void OnEnter()
        {
            ShowCanvasGroup();
        }
        
        /// <summary>
        /// 启用 UI
        /// </summary>
        public virtual void OnViewEnable()
        {
            CanvasGroup.interactable = true;
        }

        /// <summary>
        /// 禁用 UI（会继续显示但不能交互）
        /// </summary>
        public virtual void OnViewDisable()
        {
            CanvasGroup.interactable = false;
        }
        
        /// <summary>
        /// 退出当前Panel
        /// </summary>
        public virtual void OnExit()
        {
            HideCanvasGroup();
        }

        /// <summary>
        /// UI暂停交互（会继续显示但不能交互）
        /// </summary>
        [Obsolete("OnPause is Obsolete, Use OnViewDisable instead")]
        public virtual void OnPause()
        {
            CanvasGroup.interactable = false;
        }

        /// <summary>
        /// 恢复UI交互
        /// </summary>
        [Obsolete("OnResume is Obsolete, Use OnViewEnable instead")]
        public virtual void OnResume()
        {
            CanvasGroup.interactable = true;
        }
        
        private Action _onDestroyCallback;

        /// <summary>
        /// 监听Panel的销毁，在销毁时执行指定的行为。
        /// </summary>
        /// <param name="onDestroy"></param>
        internal void AddOnDestroyListener(Action onDestroy)
        {
            _onDestroyCallback = onDestroy;
        }
        protected virtual void OnDestroy()
        {
            _onDestroyCallback?.Invoke();
        }
    }
}