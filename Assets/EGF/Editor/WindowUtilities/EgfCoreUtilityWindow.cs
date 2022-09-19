using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EGF.Editor
{
    public class EgfCoreUtilityWindow : OdinMenuEditorWindow
    {
        [MenuItem(EgfEditorUtility.MenuToolMenu+"Open Utilities")]
        [MenuItem(EgfEditorUtility.AssetsToolMenu+"Open Utilities")]
        private static void OpenWindow()
        {
            var window = GetWindow<EgfCoreUtilityWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Main Window", this, EditorIcons.House},
            };
            // 获取 BaseEgfUtilityWindow 所有子类并自动
            var windowTypes = CodeReflection.GetSubClassTypes(typeof(BaseEgfUtilityWindow));
            foreach (var window in windowTypes)
            {
                var instance = Activator.CreateInstance(window);
                if (instance is BaseEgfUtilityWindow data)
                {
                    data.OnWindowCreated();
                    tree.Add(window.Name, instance);
                }
            }
            return tree;
        }
        
        
    }
}
