using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    public static class EGFDefaultSetting
    {
        /// <summary>
        /// EGF 全局宏定义
        /// </summary>
        public static class ScriptingDefineSymbols
        {
            // 日志打印过滤
            public const string LogInfoEnable = "ENABLE_INFO_LOG";
            public const string LogDebugEnable = "ENABLE_DEBUG_LOG";
            public const string LogWarningEnable = "ENABLE_WARNING_LOG";
            public const string LogErrorEnable = "ENABLE_ERROR_LOG";
            public const string LogFatalEnable = "ENABLE_FATAL_LOG";
            
            // 渲染管线
            public const string BuildInRP = "EGF_BuildInRP";
            public const string UniversalRP = "EGF_UniversalRP";
            public const string HighDefinitionRP = "EGF_HighDefinitionRP";
        }
    }
}
