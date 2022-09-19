using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// 挂在游戏物体上可以用于观察EGF已经载入的模块
    /// </summary>
    public class EgfModuleVisualize : MonoSingleton<EgfModuleVisualize>
    {
        [HideLabel,ReadOnly,Multiline(6)]
        public string describe = "Egf Module Visualize 组件" +
                                 "\n用于显示 EGF模块容器 已经载入的模块代码，调试期间能够观察对应功能模块是否正确载入。" +
                                 "\n没有该组件，Egf模块容器也能正常工作。";
        
        [ShowInInspector,Title("Loaded Module")]
        private Dictionary<string, object> egfModules = new Dictionary<string, object>();
        
        void Start()
        {
            EgfEntry.GetModule<IEventSystem>()?.AddEventListener(EgfEntry.EgfModuleChanged,RefreshLoadedModuleName);
            EgfEntry.trackEgfModuleChange = true;
            RefreshLoadedModuleName();
        }

        private void RefreshLoadedModuleName()
        {
            //loadedModule = EgfEntry.Modules;
            egfModules.Clear();
            var modules = EgfEntry.Modules;
            foreach (var module in modules)
            {
                string moduleName = module.Key.Name;
                egfModules.Add(moduleName,module.Value);
            }
        }

        protected override void Initialization()
        {
            
        }

        protected override void Release()
        {
            EgfEntry.trackEgfModuleChange = false;
            var eventSystem = EgfEntry.GetModule<IEventSystem>();
            EgfEntry.GetModule<IEventSystem>()?.RemoveEventListener(EgfEntry.EgfModuleChanged,RefreshLoadedModuleName);
        }
    }
}
