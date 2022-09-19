using System;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// EGF功能模块的获取入口，一个IOC容器
    /// </summary>
    public static class EgfEntry
    {
        // 使用说明：
        // 想加入该调用入口的模块，Awake时调用 EGFEntry.RegisterModule<T>(this); 即可。T 为提供服务的接口
        // 泛型 T 是模块希望提供的服务接口

        private static Dictionary<Type, object> modules;
        public static Dictionary<Type, object> Modules => modules;

        #region EGF开发进度信息

        internal const string ObsoleteFunction = "已过时的方法，将在未来版本弃用。";

        /// <summary>
        /// EGF模块变动时会触发的事件名
        /// </summary>
        internal const string EgfModuleChanged = "EGFModuleChangedEvent";
        internal static bool trackEgfModuleChange = false;
        
        #endregion
        

        public static void RegisterModule<T>(T instance)
        {
            if (modules == null)
                modules = new Dictionary<Type, object>();
            
            if (modules.ContainsKey(typeof(T)))
            {
                Logcat.Warning(null,$"Module <{typeof(T)}> is already registered, add action failed");
                return;
            }
            
            modules.Add(typeof(T),instance);
            Logcat.Info(null,$"Module <{typeof(T)}> added. ");
            
            if(trackEgfModuleChange)
                GetModule<IEventSystem>()?.EventTrigger(EgfModuleChanged);
        }

        public static T GetModule<T>() where T: class
        {
            if (modules.ContainsKey(typeof(T)))
            {
                T result =  modules[typeof(T)] as T;
                return result;
            }
            Logcat.Debug(null,$"Getting Module <{typeof(T)}> failed, RegisterModule it before using it");
            return null;
        }

        public static void UnRegisterModule<T>()
        {
            if (!modules.ContainsKey(typeof(T)))
            {
                Logcat.Debug(null,$"Removing Module <{typeof(T)}> failed, not registered before. ");
                return;
            }
            
            modules.Remove(typeof(T));
            Logcat.Info(null,$"Module <{typeof(T)}> was removed. ");
            
            if(trackEgfModuleChange)
                GetModule<IEventSystem>()?.EventTrigger(EgfModuleChanged);
        }
    }
}
