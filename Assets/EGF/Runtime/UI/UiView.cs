/* Created by Eric-C.
 * 基本的UI页面脚本
 *
 * 生命周期：
 *
 * ......Start()
 * 
 * OnEnter()        显示
 * OnViewEnable()   启用交互
 * OnViewDisable()  禁用交互
 * OnExit()         隐藏
 *
 * OnDisable()......
 * 
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
        public virtual void OnEnter(Action onComplete)
        {
            ShowCanvasGroup();
            onComplete();
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
        public virtual void OnExit(Action onComplete)
        {
            HideCanvasGroup();
            onComplete();
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
    }
}