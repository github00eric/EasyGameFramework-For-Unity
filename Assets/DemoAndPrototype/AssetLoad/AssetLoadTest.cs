using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EGF.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AssetLoadTest : MonoBehaviour
{
    public AssetReference scene;

    private void Awake()
    {
        Addressables.InitializeAsync();
        _goRefs = new Queue<GameObject>();
        Debug.Log(go.RuntimeKey == go2.RuntimeKey);
        Debug.Log(go.RuntimeKey.ToString() == go2.RuntimeKey.ToString());
        Debug.Log(go.RuntimeKey.ToString() == go.AssetGUID);
    }

    #region 场景加载
    
    [Button]
    void AddScene()
    {
        EgfEntry.GetModule<IAssetLoader>()?.AddScene(scene);
    }

    [Button]
    void RemoveScene()
    {
        // BUG: 目前卸载有异常
        EgfEntry.GetModule<IAssetLoader>()?.RemoveScene(scene);
    }

    // unity 直接使用 Addressable，无问题
    private AsyncOperationHandle<SceneInstance> _handler;
    [Button]
    void AddSceneUnity()
    {
        _handler = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive, true);
    }
    
    [Button]
    void RemoveSceneUnity()
    {
        Addressables.UnloadSceneAsync(_handler);
    }
    #endregion

    #region 对象加载

    public AssetReferenceT<GameObject> go;
    public AssetReferenceT<GameObject> go2;
    private Queue<GameObject> _goRefs;

    async UniTaskVoid Instantiate(AssetReferenceT<GameObject> obj)
    {
        var assetLoader = EgfEntry.GetModule<IAssetLoader>();
        if(assetLoader == null) return;
        
        var item = await assetLoader.InstantiateAsync(obj);
        _goRefs.Enqueue(item);
    }
    
    [Button]
    void AddGameObject()
    {
        Instantiate(go).Forget();
    }

    [Button]
    void DestroyObj()
    {
        if(_goRefs.Count < 1) return;
        
        var item = _goRefs.Dequeue();
        if(item)
            Destroy(item);
    }
    
    [Button]
    void AddGameObject2()
    {
        Instantiate(go2).Forget();
    }

    [Button]
    void ForceRelease1()
    {
        go.ReleaseAsset();
    }
    
    [Button]
    void ForceRelease2()
    {
        go2.ReleaseAsset();
    }

    #endregion
}
