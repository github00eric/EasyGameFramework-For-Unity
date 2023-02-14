/*
 * 全局设置页面
 * 通过改变项目宏定义来设置EGF部分功能的启用和关闭
 *
 * 请在 ..[EGF]/Runtime/Base/EGFDefaultSetting.cs 文件中查看所有宏定义
 */
#if UNITY_EDITOR && ODIN_INSPECTOR

using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EGF.Editor
{
    public class EgfSettings : BaseEgfUtilityWindow
    {
        public override void OnWindowCreated()
        {
            
        }

        [Title("EGF全局设置页面")]
        [DisplayAsString(false), HideLabel, ShowInInspector]
        public string Describe = "全局设置页面通过改变项目宏定义来设置EGF部分功能的启用和关闭。";
        
        [DisplayAsString,HideLabel,ShowInInspector]
        public string WindowMessage = "";
        /// <summary>
        /// 窗口消息打印
        /// </summary>
        /// <param name="message"></param>
        private void WindowLog(object message)
        {
            Debug.Log(message);
            WindowMessage = message.ToString();
        }

        #region 日志打印设置

        [TabGroup("日志打印设置")]
        [Button("允许所有类型日志打印",ButtonSizes.Medium),GUIColor(0.25f,0.88f,88f)]
        private void EnableAllLog()
        {
            WindowLog("开始修改编辑器宏定义设置，请稍等......");
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogInfoEnable);
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogDebugEnable);
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogWarningEnable);
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogErrorEnable);
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogFatalEnable);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        [TabGroup("日志打印设置")]
        [Button("禁用所有类型日志打印",ButtonSizes.Medium),GUIColor(0.85f,0.5f,0.12f)]
        private void DisableAllLog()
        {
            WindowLog("开始修改编辑器宏定义设置，请稍等......");
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogInfoEnable);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogDebugEnable);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogWarningEnable);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogErrorEnable);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogFatalEnable);
            WindowLog("编辑器宏定义设置修改完成。");
        }

        [TabGroup("日志打印设置")]
        [Button("允许消息类型日志 'Info' 打印")]
        private void EnableInfoLog()
        {
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogInfoEnable);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        [TabGroup("日志打印设置")]
        [Button("允许调式类型日志 'Debug' 打印")]
        private void EnableDebugLog()
        {
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogDebugEnable);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        [TabGroup("日志打印设置")]
        [Button("允许警告类型日志 'Warning' 打印")]
        private void EnableWarningLog()
        {
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogWarningEnable);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        [TabGroup("日志打印设置")]
        [Button("允许错误类型日志 'Error' 打印")]
        private void EnableErrorLog()
        {
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogErrorEnable);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        [TabGroup("日志打印设置")]
        [Button("允许致命错误类型日志 'Fatal' 打印")]
        private void EnableFatalLog()
        {
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.LogFatalEnable);
            WindowLog("编辑器宏定义设置修改完成。");
        }

        #endregion

        #region 渲染管线适配设置

        [TabGroup("渲染管线适配设置")]
        [Button("启用标准渲染管线适配")]
        private void SetDefaultRendererSetting()
        {
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.BuildInRP);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.UniversalRP);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.HighDefinitionRP);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        [TabGroup("渲染管线适配设置")]
        [Button("启用URP管线适配"),GUIColor(0.25f,0.88f,88f)]
        private void SetUniversalRendererSetting()
        {
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.BuildInRP);
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.UniversalRP);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.HighDefinitionRP);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        [TabGroup("渲染管线适配设置")]
        [Button("启用HDRP管线适配（EGF暂未完成HDRP适配）"),GUIColor(0.85f,0.5f,0.12f)]
        private void SetHighDefinitionRendererSetting()
        {
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.BuildInRP);
            DefineSymbolsUtility.RemoveDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.UniversalRP);
            DefineSymbolsUtility.AddDefineSymbols(EGFDefaultSetting.ScriptingDefineSymbols.HighDefinitionRP);
            WindowLog("编辑器宏定义设置修改完成。");
        }
        
        

        #endregion
    }
}

#endif