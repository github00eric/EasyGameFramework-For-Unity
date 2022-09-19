using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace EGF.Editor
{
    public class PresetUtilityWindow : BaseEgfUtilityWindow
    {
        public override void OnWindowCreated()
        {
            
        }

        protected override void OnSelectionChange()
        {
            Object[] select = Selection.objects;
            if (select != null)
            {
                applyPresetObjs = new List<Object>(select);
            }
            else
            {
                applyPresetObjs.Clear();
            }
        }
        
        [Title("选择需要的预设文件。"),OnValueChanged("OnPresetValueChanged")]
        public Preset preset;

        [ReadOnly][HideLabel]
        public string presetType = "请设置预设文件。";

        [Title("Project窗口下，选中所有需要应用预设文件的文件。")]
        [ShowInInspector][ReadOnly]
        public List<Object> applyPresetObjs;
        
        [Button]
        private void ApplyPreset()
        {
            if (applyPresetObjs.Count == 0 || preset == null)
            {
                Debug.Log("设置信息不足，请补充完整预设文件和待设置的文件。");
                return;
            }
            foreach (var source in applyPresetObjs)
            {
                if (preset.ApplyTo(source))
                {
                    Debug.Log($"{source} 预设应用成功。");
                }
                else
                {
                    Debug.Log($"{source} 预设应用失败。");
                }
            }
        }
        
        private void OnPresetValueChanged()
        {
            if (preset != null)
                presetType = "选中的 Preset 可以应用于：" + preset.GetTargetFullTypeName();
            else
                presetType = "请设置预设文件。";
        }
    }
}
