/*
 * 替换和扩展Unity原生Debug的一些功能
 * TODO: 将Debug消息可视化到GUI中
 */

using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace EGF.Runtime
{
	public static class Logcat
	{
		
		public static readonly string[] LogScriptingDefineSymbols = new string[5]
        {
	        "ENABLE_INFO_LOG",
	        "ENABLE_DEBUG_LOG",
	        "ENABLE_WARNING_LOG",
	        "ENABLE_ERROR_LOG",
	        "ENABLE_FATAL_LOG"
        };
		
		#region 日志记录

		private const string DefaultColor = "#00BCE7";
		private const string DebugColor = "#FFFFFF";	// 白色标记
		private const string WarningColor = "#FF9700";	// 橙色标记
		private const string ErrorColor = "#FF0000";	// 红色标记
		private const string FatalColor = "#FF00FF";	// 紫色标记

		[Conditional(EGFDefaultSetting.ScriptingDefineSymbols.LogInfoEnable)]
		public static void Info(object sender, object message)
		{
			string senderInformation = sender == null ? "unknown" : sender.ToString();
			string temp = $"Info: {senderInformation}: " + message;
			UnityEngine.Debug.Log(temp);
		}
		
		[Conditional(EGFDefaultSetting.ScriptingDefineSymbols.LogDebugEnable)]
		public static void Debug(object sender, object message)
		{
			string senderInformation = sender == null ? "unknown" : sender.ToString();
			string temp = $"<color={DebugColor}>Debug: </color>" +
			              $"<color={DefaultColor}>{senderInformation}: </color>" + 
			              message;
			UnityEngine.Debug.Log(temp);
		}

		[Conditional(EGFDefaultSetting.ScriptingDefineSymbols.LogWarningEnable)]
		public static void Warning(object sender, object message)
		{
			string senderInformation = sender == null ? "unknown" : sender.ToString();
			string temp = $"<b><color={WarningColor}>Warning: </color>" +
			              $"<color={DefaultColor}>{senderInformation}: </color></b>" + 
			              message;
			UnityEngine.Debug.LogWarning(temp);
		}
		
		[Conditional(EGFDefaultSetting.ScriptingDefineSymbols.LogErrorEnable)]
		public static void Error(object sender, object message)
		{
			string senderInformation = sender == null ? "unknown" : sender.ToString();
			string temp = $"<b><color={ErrorColor}>Error: </color>" +
			              $"<color={DefaultColor}>{senderInformation}: </color></b>" + 
			              message;
			UnityEngine.Debug.LogError(temp);
		}
		
		[Conditional(EGFDefaultSetting.ScriptingDefineSymbols.LogFatalEnable)]
		public static void Fatal(object sender, object message)
		{
			string senderInformation = sender == null ? "unknown" : sender.ToString();
			string temp = $"<b><color={FatalColor}>Fatal Error: </color>" +
			              $"<color={DefaultColor}>{senderInformation}: </color></b>" + 
			              message;
			UnityEngine.Debug.LogError(temp);
		}

		#endregion
	}
}

