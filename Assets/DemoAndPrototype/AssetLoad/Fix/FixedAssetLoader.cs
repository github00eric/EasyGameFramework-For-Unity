using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public static class FixedAssetLoader
{
    private static readonly Dictionary<object, AsyncOperationHandle<GameObject>> _assets;
    private static readonly Dictionary<object, UniTaskCompletionSource<GameObject>> _loadingTasks;

    static FixedAssetLoader()
    {
        _assets = new Dictionary<object, AsyncOperationHandle<GameObject>>();
        _loadingTasks = new Dictionary<object, UniTaskCompletionSource<GameObject>>();
    }

    public static async UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> reference)
    {
        var go = await InstantiateAsyncInternal(reference);
        return go;
    }

    static async UniTask<GameObject> InstantiateAsyncInternal(AssetReferenceT<GameObject> reference, Transform parent = null,
        bool inWorldSpace = false)
    {
        if (!reference.RuntimeKeyIsValid())     // 检查资源有效性
        {
            Debug.LogWarning("Invalid asset reference.");
            return null;
        }

        var obj = await LoadAssetAsync(reference);
        if (!obj) return null;

        var instance = Object.Instantiate(obj, parent, inWorldSpace);
        AddAutoReleaseAssetTrigger(reference, instance);
        return instance;
    }

    static async UniTask<GameObject> LoadAssetAsync(AssetReferenceT<GameObject> reference)
    {
        var key = reference.RuntimeKey;
        Debug.Log(_assets.ContainsKey(key));
        // 1. 先检查资源是否有缓存
        if (_assets.ContainsKey(key) && _assets[key].Status == AsyncOperationStatus.Succeeded && _assets[key].Result)
            return _assets[key].Result;
        
        // 2. 检查一下是否有其它异步操作正在加载资源
        UniTaskCompletionSource<GameObject> loadingTask;
        if (_loadingTasks.ContainsKey(key))
        {
            // 有加载中的，可以直接等待加载完成并复用
            loadingTask = _loadingTasks[key];
        }
        else
        {
            loadingTask = new UniTaskCompletionSource<GameObject>();
            _loadingTasks.Add(key, loadingTask);
            
            // 3. 执行异步加载
            var handle = Addressables.LoadAssetAsync<GameObject>(reference);
            handle.Completed += operationHandle => HandleLoadingComplete(reference, operationHandle);
            _assets.Add(key, handle);
        }
        
        // 等待加载操作完成
        return await loadingTask.Task;
    }
    
    private static void HandleLoadingComplete(AssetReferenceT<GameObject> reference, AsyncOperationHandle<GameObject> handle)
    {
        var key = reference.RuntimeKey;
        // 从 _loadingTasks 中获取 UniTaskCompletionSource 对象，并将加载的结果设置为其结果值
        var loadingTask = _loadingTasks[key];
        loadingTask.TrySetResult(handle.Result);

        // 移除当前加载任务
        _loadingTasks.Remove(key);
    }

    #region Auto Release
    
    static void AddAutoReleaseAssetTrigger(AssetReference assetReference, GameObject targetGO)
    {
        targetGO.AddComponent<HandleOnDestroy>().OnDestroyEvent +=
            () => ReleaseAsset(assetReference);
    }

    static void ReleaseAsset(AssetReference assetReference)
    {
        if(!assetReference.RuntimeKeyIsValid()) return;
        
        var key = assetReference.RuntimeKey;
        if (!_assets.ContainsKey(key))
            return;
        
        var handle = _assets[key];
        _assets.Remove(key);
        Addressables.Release(handle);
    }

    #endregion
}