/*
 * EditorSelectionUtility
 * 编辑器下的选择对象代码工具
 */
using System.Collections.Generic;
using EGF.Runtime;
using UnityEditor;
using UnityEngine;

namespace EGF.Editor
{
    /// <summary>
    /// 编辑器下的选择对象代码工具
    /// </summary>
    public static class EditorSelectionUtility
    {
        public static void GetSelectedGameObjects_InProjectWindow(out List<GameObject> results)
        {
            GetSelectedObjects_InProjectWindow<GameObject>(out var temps);
            if(temps==null)
                Logcat.Warning(null,"No Selected GameObjects. ");

            results = temps;
        }

        public static void GetSelectedObjects_InProjectWindow<T>(out List<T> result) where T: Object
        {
            GetSelectedObjectsAddress_InProjectWindow(out var addresses);
            var temp = new List<T>();
            foreach (var address in addresses)
            {
                T loadAsset = AssetDatabase.LoadAssetAtPath(address, typeof(T)) as T;
                if(loadAsset)
                    temp.Add(loadAsset);
            }
            
            result = temp;
        }

        public static void GetSelectedObjectsAddress_InProjectWindow(out string[] addressArgs)
        {
            string[] guids = Selection.assetGUIDs; // 获取选中文件guid
            if (guids.Length <= 0)
            {
                addressArgs = null;
                return;
            }

            List<string> temp = new List<string>();
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                temp.Add(assetPath);
            }

            addressArgs = temp.ToArray();
        }
    }
}
