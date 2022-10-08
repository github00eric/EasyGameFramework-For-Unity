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
        
        [Title("Introduction",null,TitleAlignments.Centered)]
        [DisplayAsString(false),HideLabel]
        public string mainIntroduce = "\n" +
                                      "Welcome to use Easy Game Framework. \n" +
                                      "You can get quick tutorial from tab pages below. \n" +
                                      "\n欢迎使用 Easy Game Framework。\n" +
                                      "你可以在下方页签中找到对应功能的快速教程。\n";
        
        [TabGroup("EGFEntry")]
        [DisplayAsString(false),HideLabel]
        public string introduce1 = "\nAbout EGFEntry:";
        
        [TabGroup("IEventSystem | 事件系统")]
        [DisplayAsString(false),HideLabel]
        public string introduce2 = "\nAbout IEventSystem:";
        
        // Line2-Utilities 介绍
        [TabGroup("Utilities", "Coordinate | 坐标工具")]
        [DisplayAsString(false),HideLabel]
        public string introduce3 = "\nUse Coordinate to transform position " +
                                   "between world-coordinate, screen-coordinate, ui-coordinate. \n" +
                                   "\n使用 Coordinate 计算空间坐标在世界坐标系、屏幕坐标系、ui坐标系的变换。";
    }
}
