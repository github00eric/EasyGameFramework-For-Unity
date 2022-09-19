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
        private CanvasGroup canvasGroup;
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
        /// 显示和激活当前Panel交互
        /// </summary>
        public virtual void OnEnter()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// 退出当前Panel
        /// </summary>
        public virtual void OnExit()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// UI暂停交互（会继续显示但不能交互）
        /// </summary>
        public virtual void OnPause()
        {
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 恢复UI交互
        /// </summary>
        public virtual void OnResume()
        {
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
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