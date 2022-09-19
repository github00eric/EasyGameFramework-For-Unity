using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using AddressablesMaster;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace EGF.Runtime
{
    public class AssetsLoader : MonoSingleton<AssetsLoader>,IAsset
    {
        [HideLabel,ReadOnly,Multiline(5)]
        public string describe = "资源加载模块，提供模块接口 > IAsset" +
                                 "\n使用 Load 载入资源" +
                                 "\n使用 Instantiate 实例化游戏对象" +
                                 "\n使用 SwitchScene 切换场景" +
                                 "\n使用 AddScene 动态添加场景" +
                                 "\n使用 RemoveScene 动态移除场景";

        // TODO：
        // 1. 是否需要对象池复用？
        // 2. 加载资源 -> 跳转场景 -> 加载资源：有几率出现 Attempting to load AssetReference that has already been loaded.
        // 3. 切换场景，场景中已有预制体的 scriptable object 配置会丢失。

        private int _sceneHandle;    // current loaded scene
        
        [ShowInInspector,ReadOnly] private Dictionary<string, Object> _prototypeAssets = new Dictionary<string, Object>();
        [ShowInInspector] private GameObject _releaseAssetTrigger;    // HACK: 不确定有没有效果，待测试
        
        protected override void Initialization()
        {
            ManageAddressables.Initialize();
            EgfEntry.RegisterModule<IAsset>(this);
        }

        protected override void Release()
        {
            EgfEntry.UnRegisterModule<IAsset>();
        }

        #region IAsset
        
        public T Load<T>(string path) where T : Object
        {
            if (_prototypeAssets.ContainsKey(path))
            {
                return _prototypeAssets[path] as T;
            }
            var result = ManageAddressables.LoadAssetSync<T>(path);
            _prototypeAssets.Add(path,result);
            AddAutoReleaseTrigger(path);
            return result;
        }

        public T Load<T>(AssetReferenceT<T> reference) where T : Object
        {
            string key = reference.AssetGUID;
            if (_prototypeAssets.ContainsKey(key))
            {
                return _prototypeAssets[key] as T;
            }
            var result = ManageAddressables.LoadAssetSync(reference);
            _prototypeAssets.Add(key,result);
            AddAutoReleaseTrigger(reference);
            return result;
        }

        public void LoadAsync<T>(string path, Action<T> onCompletion) where T : Object
        {
            if (_prototypeAssets.ContainsKey(path))
            {
                var result = _prototypeAssets[path] as T;
                onCompletion.Invoke(result);
            }
            ManageAddressables.LoadAssetAsync<T>(path, (value) =>
            {
                _prototypeAssets.Add(path,value);
                AddAutoReleaseTrigger(path);
                onCompletion.Invoke(value);
            });
        }

        public void LoadAsync<T>(AssetReferenceT<T> reference, Action<T> onCompletion) where T : Object
        {
            string key = reference.AssetGUID;
            if (_prototypeAssets.ContainsKey(key))
            {
                var result = _prototypeAssets[key] as T;
                onCompletion.Invoke(result);
            }
            ManageAddressables.LoadAssetAsync(reference, (value) =>
            {
                _prototypeAssets.Add(key,value);
                AddAutoReleaseTrigger(reference);
                onCompletion.Invoke(value);
            });
        }

        public GameObject Instantiate(string path)
        {
            return ManageAddressables.InstantiateSyncWithAutoRelease(path);
        }

        public GameObject Instantiate(AssetReferenceT<GameObject> reference)
        {
            return ManageAddressables.InstantiateSyncWithAutoRelease(reference);
        }

        public void InstantiateAsync(string path, Action<GameObject> onCompletion)
        {
            ManageAddressables.InstantiateAsyncWithAutoRelease(path, onCompletion: onCompletion);
        }

        public void InstantiateAsync(AssetReferenceT<GameObject> reference, Action<GameObject> onCompletion)
        {
            ManageAddressables.InstantiateAsyncWithAutoRelease(reference, onCompletion: onCompletion);
        }

        #region Scene 场景

        public async void SwitchScene(string scenePath)
        {
            ReleasePrototypeAssets();
            // await ManageAddressables.LoadSceneAsync(scenePath);
            try
            {
                var operation = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Single, true, 100);
                await operation.Task;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddScene(string scenePath)
        {
            ManageAddressables.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
        }

        public void RemoveScene(string scenePath)
        {
            ManageAddressables.UnloadSceneAsync(scenePath);
        }

        public async void SwitchScene(AssetReference sceneReference)
        {
            ReleasePrototypeAssets();
            // await ManageAddressables.LoadSceneAsync(sceneReference);
            try
            {
                var operation = Addressables.LoadSceneAsync(sceneReference);
                await operation.Task;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddScene(AssetReference sceneReference)
        {
            ManageAddressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);
        }

        public void RemoveScene(AssetReference sceneReference)
        {
            ManageAddressables.UnloadSceneAsync(sceneReference);
        }

            #endregion

        #endregion

        private void ReleasePrototypeAssets()
        {
            _prototypeAssets.Clear();
            Destroy(_releaseAssetTrigger);
        }

        private void AddAutoReleaseTrigger(string key)
        {
            if (!_releaseAssetTrigger)
            {
                _releaseAssetTrigger = new GameObject("AssetReleaseTrigger");
                _releaseAssetTrigger.SetActive(false);
            }
            ManageAddressables.AddAutoReleaseAssetTrigger(key,_releaseAssetTrigger);
        }
        private void AddAutoReleaseTrigger<T>(AssetReferenceT<T> key) where T : Object
        {
            if (!_releaseAssetTrigger)
            {
                _releaseAssetTrigger = new GameObject("AssetReleaseTrigger");
                _releaseAssetTrigger.SetActive(false);
            }
            ManageAddressables.AddAutoReleaseAssetTrigger(key,_releaseAssetTrigger);
        }
    }
}
