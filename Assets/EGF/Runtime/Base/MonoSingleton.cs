using UnityEngine;

namespace EGF.Runtime
{
    /// <summary>
    /// MonoBehaviour脚本单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        public static T Instance => instance;

        public bool dontDestroyOnLoad;

        private void Awake()
        {
            if (instance != null)
            {
                Logcat.Warning(this,$"There is another {typeof(T)} singleton, Trying add new failed");
                Destroy(gameObject);
                return;
            }
            
            instance = GetComponent<T>();
            
            Initialization();
            
            if (dontDestroyOnLoad)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            Logcat.Info(this,$"MonoSingleton {typeof(T)} Initialized");
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                Release();
                instance = null;
                Logcat.Info(this,$"{typeof(T)} singleton destroyed");
            }
        }
        
        protected abstract void Initialization();

        protected abstract void Release();
    }
}
