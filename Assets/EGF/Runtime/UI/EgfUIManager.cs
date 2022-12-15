/*
 * EgfUIManager
 * UI模块
 * 
 * 提供 UI 功能接口 > IUIManager
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if EGF_UniversalRP
using UnityEngine.Rendering.Universal;
#endif

namespace EGF.Runtime
{
    public class EgfUIManager : MonoSingleton<EgfUIManager>,IUIManager
    {
        // 需要确保，有预制体配置
        [Tooltip("希望 ui系统 生成的 Canvas，| Canvas的 RendererMode 如果不为 overlay，请将 ui相机包含在预制体中。")]
        public Canvas defaultCanvas;

        /// <summary>
        /// 场景画布缓存
        /// </summary>
        Canvas canvas;
        Transform canvasTransform;

        /// <summary>
        /// Ui相机缓存
        /// </summary>
        Camera uiCamera;

        // Dictionary<string, UiView> floatPanelObjDic;

        /// <summary>
        /// 存放激活的 Panel 列表的栈
        /// </summary>
        [Header("Panel Active:")]
        [Tooltip("可查看当前已载入的UI页面和层级关系")]
        Stack<UiView> panelStack;
        
        protected override void Initialization()
        {
            panelStack = new Stack<UiView>();
            canvas = GetCanvasInternal();
            canvasTransform = canvas.transform;

            EgfEntry.RegisterModule<IUIManager>(this);
        }

        protected override void Release()
        {
            panelStack.Clear();
            EgfEntry.UnRegisterModule<IUIManager>();
        }
        
        
        /// <summary>
        /// 内部调用：获得指定Panel的脚本,但不显示
        /// </summary>
        /// <param name="panelKey">加载Panel使用的 资源Key</param>
        /// <returns></returns>
        private UiView GetPanel(string panelKey)
        {
            IAssetLoader assetLoader = EgfEntry.GetModule<IAssetLoader>();
            if (assetLoader == null)
            {
                Logcat.Error(this,"缺少资源加载模块 IAssetLoader，请向EgfEntry添加 IAssetLoader 模块。");
                return null;
            }
            // 缓存中没有 UiView 则使用 IAssets 加载
            UiView bp = assetLoader.Instantiate(panelKey)?.GetComponent<UiView>();
            if (!bp)
            {
                Logcat.Warning(this,$"加载 UiView {panelKey} 失败。检查指定资源是否标记为可寻址，且添加了UIPanel脚本。");
                return null;
            }

            bp.transform.SetParent(canvasTransform, false);
            bp.HideCanvasGroup();
            
            Logcat.Info(this,$"加载 Panel {bp.name} 成功. ");
            return bp;
        }

        private Canvas GetCanvasInternal()
        {
            Canvas result = canvas;
            if (result != null) return result;
            
            Logcat.Info(this,"场景缺少画布，UI自动创建画布中");
            var temp = Instantiate(defaultCanvas,transform,false);
                
            if (temp.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                if (temp.worldCamera == null)
                {
                    Logcat.Warning(this,"画布模板使用渲染模式不为 ScreenSpaceOverlay 时，需要在预制体中准备好 UI相机，否则可能出错。");
                }
                uiCamera = temp.worldCamera;
            }
            result = temp;
            return result;
        }

        private Camera GetUICameraInternal()
        {
            Camera result = uiCamera;
            return result;
        }

        private void UpdateUICamera(Camera newUICamera, Camera viewCamera)
        {
            uiCamera = newUICamera;
#if EGF_BuildInRP || !EGF_UniversalRP
            uiCamera.depth = viewCamera.depth + 1;
#endif
#if EGF_UniversalRP
            
            var cameraData = uiCamera.GetUniversalAdditionalCameraData();
            cameraData.renderType = CameraRenderType.Overlay;
            
            var mainCameraData = viewCamera.GetUniversalAdditionalCameraData();
            mainCameraData.cameraStack.Add(uiCamera);
#endif
        }
        
        private void ShowViewInternal(UiView uiInstance)
        {
            uiInstance.OnEnter(uiInstance.OnViewEnable);
            panelStack.Push(uiInstance);
        }

        private void ExitViewInternal(UiView uiInstance)
        {
            // UiView bp = panelStack.Pop();
            uiInstance.OnViewDisable();
            uiInstance.OnExit(() =>
            {
                Destroy(uiInstance.gameObject);
            });
        }
        
        // ----------------------------------------------------------------------------------------------------

        #region IUIManager

        public Camera GetUiCamera()
        {
            return GetUICameraInternal();
        }

        public Canvas GetUiCanvas()
        {
            return GetCanvasInternal();
        }

        public void SetViewCamera(Camera viewCamera)
        {
            var uiCam = GetUICameraInternal();
            if (uiCam != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                UpdateUICamera(uiCam, viewCamera);
            }
        }

        public UiView GetUi(string panelName)
        {
            return GetPanel(panelName);
        }

        public UiView Peek()
        {
            if (panelStack.Count > 0)
            {
                UiView top = panelStack.Peek();
                return top;
            }

            return null;
        }

        public UiView Show(string panelName)
        {
            if (panelStack.Count > 0)
            {
                UiView top = panelStack.Pop();
                ExitViewInternal(top);
            }
            UiView newView = GetPanel(panelName);
            ShowViewInternal(newView);
            return newView;
        }

        public UiView Show(UiView uiInstance)
        {
#if UNITY_EDITOR
            if (panelStack.Contains(uiInstance))
            {
                Logcat.Warning(this,$"请勿重复显示 {uiInstance.name}");
                return uiInstance;
            }
#endif
            if (panelStack.Count > 0)
            {
                UiView top = panelStack.Pop();
                ExitViewInternal(top);
            }
            ShowViewInternal(uiInstance);
            return uiInstance;
        }

        public UiView ShowAdditive(string panelName)
        {
            if (panelStack.Count > 0)
            {
                UiView top = panelStack.Peek();
                top.OnViewDisable();
            }
            
            UiView newView = GetPanel(panelName);
            ShowViewInternal(newView);
            return newView;
        }

        public UiView ShowAdditive(UiView uiInstance)
        {
#if UNITY_EDITOR
            if (panelStack.Contains(uiInstance))
            {
                Logcat.Warning(this,$"请勿重复显示 {uiInstance.name}");
                return uiInstance;
            }
#endif
            if (panelStack.Count > 0)
            {
                UiView top = panelStack.Peek();
                top.OnViewDisable();
            }
            ShowViewInternal(uiInstance);
            return uiInstance;
        }

        public UiView Hide()
        {
            if (panelStack.Count <= 0)
                return null;

            UiView bp = panelStack.Pop();
            ExitViewInternal(bp);

            if (panelStack.Count <= 0)
                return bp;

            UiView top = panelStack.Peek();
            if (top)
            {
                top.OnViewEnable();
            }
			
            return bp;
        }

        #endregion
    }
}
