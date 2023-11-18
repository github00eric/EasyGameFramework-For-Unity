using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace EGF.Runtime
{
    public interface IAssetLoader
    {
        // ----------------------------------------------------------------------------------------------------
        // 载入资源或游戏对象
        T Load<T>(string path) where T : Object;
        T Load<T>(AssetReferenceT<T> reference) where T : Object;
        UniTask<T> LoadAsync<T>(string path) where T : Object;
        UniTask<T> LoadAsync<T>(AssetReferenceT<T> reference) where T : Object;
        
        // ----------------------------------------------------------------------------------------------------
        // 直接载入并实例化游戏对象
        GameObject Instantiate(string path);
        
        [Obsolete("There are some issues when instantiate multiple AssetReference, use FixedAssetLoader.InstantiateAsync instead")]
        GameObject Instantiate(AssetReferenceT<GameObject> reference);
        UniTask<GameObject> InstantiateAsync(string path);
        
        [Obsolete("There are some issues when instantiate multiple AssetReference, use FixedAssetLoader.InstantiateAsync instead")]
        UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> reference);
        
        // ----------------------------------------------------------------------------------------------------
        // 添加、移出、切换场景
        
        UniTask<SceneInstance> AddScene(string scenePath, bool activateOnLoad = true);
        UniTask<SceneInstance> AddScene(AssetReference sceneReference, bool activateOnLoad = true);
        
        UniTask RemoveScene(string scenePath);
        UniTask RemoveScene(AssetReference sceneReference);
        
        UniTask<SceneInstance> Switch2Scene(string scenePath, Action<float> progressHandler, bool activateOnLoad = true);
        UniTask<SceneInstance> Switch2Scene(AssetReference sceneReference, Action<float> progressHandler, bool activateOnLoad = true);
    }
}
