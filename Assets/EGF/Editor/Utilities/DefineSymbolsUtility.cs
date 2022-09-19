/*
 * DefineSymbolsUtility
 * 用于处理宏定义的编辑器代码工具
 */
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace EGF.Editor
{
    /// <summary>
    /// 用于处理宏定义的编辑器代码工具
    /// </summary>
    public static class DefineSymbolsUtility
    {
        static readonly BuildTargetGroup[] BuildTargetGroups = new BuildTargetGroup[4]
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Android,
            BuildTargetGroup.WebGL
        };
        
        private static string[] GetDefineSymbols(BuildTargetGroup buildTargetGroup)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            string[] symbolsList = symbols.Split(';');
            return symbolsList;
        }

        private static void SetDefineSymbols(string[] symbols,BuildTargetGroup buildTargetGroup)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup,symbols);
        }

        private static void AddDefineSymbols(string targetDefineSymbols,BuildTargetGroup buildTargetGroup)
        {
            bool needAdd = true;
            var symbols = GetDefineSymbols(buildTargetGroup);

            List<string> newSymbolsList = new List<string>();
            foreach (var symbol in symbols)
            {
                if (symbol == targetDefineSymbols)
                {
                    needAdd = false;
                    break;
                }
                newSymbolsList.Add(symbol);
            }
            
            if (!needAdd) return;

            newSymbolsList.Add(targetDefineSymbols);
            var newSymbols = newSymbolsList.ToArray();
            SetDefineSymbols(newSymbols,buildTargetGroup);
        }

        private static void RemoveDefineSymbols(string targetDefineSymbols,BuildTargetGroup buildTargetGroup)
        {
            bool needRemove = false;
            var symbols = GetDefineSymbols(buildTargetGroup);

            List<string> newSymbolsList = new List<string>();
            foreach (var symbol in symbols)
            {
                if (symbol == targetDefineSymbols)
                    needRemove = true;
                else
                {
                    newSymbolsList.Add(symbol);
                }
            }
            
            if(!needRemove) return;

            var newSymbols = newSymbolsList.ToArray();
            SetDefineSymbols(newSymbols,buildTargetGroup);
        }

        #region API

        /// <summary>
        /// 为主要编译目标平台添加宏定义，仅编辑器下可用
        /// </summary>
        /// <param name="targetDefineSymbols"></param>
        [Conditional("UNITY_EDITOR")]
        public static void AddDefineSymbols(string targetDefineSymbols)
        {
            foreach (var targetGroup in BuildTargetGroups)
            {
                AddDefineSymbols(targetDefineSymbols,targetGroup);
            }
        }

        /// <summary>
        /// 为主要编译目标平台移除宏定义，仅编辑器下可用
        /// </summary>
        /// <param name="targetDefineSymbols"></param>
        [Conditional("UNITY_EDITOR")]
        public static void RemoveDefineSymbols(string targetDefineSymbols)
        {
            foreach (var targetGroup in BuildTargetGroups)
            {
                RemoveDefineSymbols(targetDefineSymbols,targetGroup);
            }
        }

        #endregion
    }
}
