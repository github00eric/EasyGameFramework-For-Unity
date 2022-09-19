using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace EGF.Runtime
{
    public interface IAsset
    {
        T Load<T>(string path) where T : Object;
        T Load<T>(AssetReferenceT<T> reference) where T : Object;
        void LoadAsync<T>(string path, Action<T> onCompletion) where T : Object;
        void LoadAsync<T>(AssetReferenceT<T> reference, Action<T> onCompletion) where T : Object;
        // --------------------------------------------------
        GameObject Instantiate(string path);
        GameObject Instantiate(AssetReferenceT<GameObject> reference);
        void InstantiateAsync(string path, Action<GameObject> onCompletion);
        void InstantiateAsync(AssetReferenceT<GameObject> reference, Action<GameObject> onCompletion);
        // --------------------------------------------------
        // TODO: 跟踪加载进度
        void SwitchScene(string scenePath);
        void AddScene(string scenePath);
        void RemoveScene(string scenePath);
        // --------------------------------------------------
        void SwitchScene(AssetReference sceneReference);
        void AddScene(AssetReference sceneReference);
        void RemoveScene(AssetReference sceneReference);
    }
}