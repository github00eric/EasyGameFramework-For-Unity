﻿using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
#if EGF_UniversalRP
using UnityEngine.Rendering.Universal;
#endif

namespace EGF.Runtime
{
    public class UIModule : MonoSingleton<UIModule>,IUIManager
    {
        [HideLabel, ReadOnly, Multiline(8)] 
        public string describe = "UI模块，提供UI功能接口 > IUIManager";
        
        [Tooltip("场景中无 Canvas 时默认生成的 Canvas，| Canvas的 RendererMode 如果不为 overlay，请将 ui相机包含在预制体中。"),Sirenix.OdinInspector.Required,AssetsOnly]
        public Canvas defaultCanvas;
        
        /// <summary>
        /// 场景画布缓存
        /// </summary>
        [ShowInInspector,ReadOnly] Transform canvasTransform;

        /// <summary>
        /// Ui相机缓存
        /// </summary>
        [ShowInInspector, ReadOnly] Camera uiCamera;

        /// <summary>
        /// 缓存Panel游戏对象
        /// </summary>
        Dictionary<string, UiView> PanelObjDic;

        /// <summary>
        /// 存放激活的Panel列表的栈
        /// </summary>
        [Title("User Interface Active:")]
        [Tooltip("可查看当前载入的UI页面和层级关系")]
        [ShowInInspector,ReadOnly] Stack<UiView> panelStack;
        
        protected override void Initialization()
        {
            PanelObjDic = new Dictionary<string, UiView>();
            panelStack = new Stack<UiView>();
            canvasTransform = GetCanvasInternal();

            EgfEntry.RegisterModule<IUIManager>(this);
        }

        protected override void Release()
        {
            PanelObjDic.Clear();
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
            PanelObjDic.TryGetValue(panelKey, out UiView bp);
            if (bp != null) return bp;

            IAsset assetLoader = EgfEntry.GetModule<IAsset>();
            if (assetLoader == null)
            {
                Logcat.Error(this,"缺少资源加载模块IAsset，请向EgfEntry添加IAsset模块。");
                return null;
            }
            // 缓存中没有 UiView 则使用 IAssets 加载
            bp = assetLoader.Instantiate(panelKey)?.GetComponent<UiView>();
            if (bp)
            {
                bp.transform.SetParent(GetCanvasInternal(), false);
                PanelObjDic.Add(panelKey, bp);
                bp.AddOnDestroyListener(() =>
                {
                    if (PanelObjDic.ContainsKey(panelKey))
                        PanelObjDic.Remove(panelKey);
                });
                Logcat.Info(this,$"Load Panel {bp.name} success. ");
            }
            else
            {
                Logcat.Warning(this,$"加载 UiView {panelKey} 失败。检查指定资源是否标记为可寻址，且添加了UIPanel脚本。");
            }
            return bp;
        }

        private Transform GetCanvasInternal()
        {
            Transform result = canvasTransform;
            if (result == null)
            {
                Logcat.Info(this,"场景缺少画布，UI自动创建画布中");
                var temp = Instantiate(defaultCanvas);
                
                if (temp.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    if (temp.worldCamera == null)
                    {
                        Logcat.Warning(this,"画布模板使用渲染模式不为 ScreenSpaceOverlay 时，需要在预制体中准备好 UI相机，否则可能出错。");
                    }
                    UpdateUICamera(temp.worldCamera);
                }
                result = temp.transform;
            }
            return result;
        }

        private Camera GetUICameraInternal()
        {
            Camera result = uiCamera;
            return result;
        }

        private Camera CameraMain()
        {
            var result = Camera.main;
            if (result == null)
            {
                Logcat.Error(this,"未获取到场景中主相机。");
            }
            return result;
        }

        private void UpdateUICamera(Camera newUICamera)
        {
            uiCamera = newUICamera;
#if EGF_BuildInRP
            uiCamera.depth = CameraMain().depth + 1;
#endif
#if EGF_UniversalRP
            
            var cameraData = uiCamera.GetUniversalAdditionalCameraData();
            cameraData.renderType = CameraRenderType.Overlay;
            
            var mainCameraData = CameraMain().GetUniversalAdditionalCameraData();
            mainCameraData.cameraStack.Add(uiCamera);
#endif
        }

        #region IUIManager

        public UiView ShowFocus(string panelName)
        {
            //栈内当前如果有页面，就暂停当前页面
            if (panelStack.Count > 0)
            {
                UiView top = panelStack.Peek();
                top.OnPause();
            }

            UiView newView = GetPanel(panelName);
            newView.OnEnter();
            panelStack.Push(newView);
            return newView;
        }

        public UiView Show(string panelName)
        {
            UiView newView = GetPanel(panelName);
            newView.OnEnter();
            panelStack.Push(newView);
            return newView;
        }

        public UiView Hide()
        {
            if (panelStack.Count <= 0)
                return null;

            UiView bp = panelStack.Pop();
            bp.OnExit();

            if (panelStack.Count <= 0)
                return bp;

            UiView top = panelStack.Peek();
            top.OnResume();
			
            return bp;
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

        public Camera UICamera => uiCamera;

        #endregion

    }
}