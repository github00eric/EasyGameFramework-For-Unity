using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoadTestFixed : MonoBehaviour
{
    #region 对象加载

    public AssetReferenceT<GameObject> go;
    public AssetReferenceT<GameObject> go2;
    private Queue<GameObject> _goRefs;

    private void Awake()
    {
        _goRefs = new Queue<GameObject>();
        Debug.Log(go.RuntimeKey == go2.RuntimeKey);
    }

    async UniTaskVoid Instantiate(AssetReferenceT<GameObject> obj)
    {
        var item = await FixedAssetLoader.InstantiateAsync(obj);
        
        if(item)
            _goRefs.Enqueue(item);
    }

    [Button]
    public void FixedAddGameObject()
    {
        Instantiate(go).Forget();
    }
    [Button]
    public void FixedAddGameObject2()
    {
        Instantiate(go2).Forget();
    }

    [Button]
    public void DestroyObj()
    {
        if(_goRefs.Count < 1) return;
        
        var item = _goRefs.Dequeue();
        if(item)
            Destroy(item);
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
