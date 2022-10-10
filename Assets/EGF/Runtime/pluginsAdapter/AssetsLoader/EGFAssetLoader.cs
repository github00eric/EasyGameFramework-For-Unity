using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AddressablesMaster;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace EGF.Runtime
{
    // TODO：给加载资源操作添加取消
    public class EGFAssetLoader : MonoSingleton<EGFAssetLoader>,IAssetLoader
    {
        protected override void Initialization()
        {
            ManageAddressables.Initialize();
            EgfEntry.RegisterModule<IAssetLoader>(this);
        }

        protected override void Release()
        {
            EgfEntry.UnRegisterModule<IAssetLoader>();
        }
        
        // ----------------------------------------------------------------------------------------------------

        public T Load<T>(string path) where T : Object
        {
            return ManageAddressables.LoadAssetSync<T>(path);
        }

        public T Load<T>(AssetReferenceT<T> reference) where T : Object
        {
            return ManageAddressables.LoadAssetSync<T>(reference);
        }

        public async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            var task = ManageAddressables.LoadAssetAsync<T>(path).AsUniTask();
            var result = await task;

            return result.Value;
        }

        public async UniTask<T> LoadAsync<T>(AssetReferenceT<T> reference) where T : Object
        {
            var task = ManageAddressables.LoadAssetAsync(reference).AsUniTask();
            var result = await task;
            
            return result.Value;
        }

        // ----------------------------------------------------------------------------------------------------
        
        public GameObject Instantiate(string path)
        {
            return ManageAddressables.InstantiateSyncWithAutoRelease(path);
        }

        public GameObject Instantiate(AssetReferenceT<GameObject> reference)
        {
            return ManageAddressables.InstantiateSyncWithAutoRelease(reference);
        }

        public async UniTask<GameObject> InstantiateAsync(string path)
        {
            var task = ManageAddressables.InstantiateAsyncWithAutoRelease(path).AsUniTask();
            var result = await task;

            return result;
        }

        public async UniTask<GameObject> InstantiateAsync(AssetReferenceT<GameObject> reference)
        {
            var task = ManageAddressables.InstantiateAsyncWithAutoRelease(reference).AsUniTask();
            var result = await task;

            return result;
        }
        
        // ----------------------------------------------------------------------------------------------------

        public async UniTask<SceneInstance> AddScene(string scenePath, bool activateOnLoad = true)
        {
            var task = ManageAddressables.LoadSceneAsync(scenePath, LoadSceneMode.Additive, activateOnLoad).AsUniTask();
            var result = await task;

            return result.Value;
        }

        public async UniTask<SceneInstance> AddScene(AssetReference sceneReference, bool activateOnLoad = true)
        {
            var task = ManageAddressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive, activateOnLoad).AsUniTask();
            var result = await task;

            return result.Value;
        }

        public UniTask RemoveScene(string scenePath)
        {
            return ManageAddressables.UnloadSceneAsync(scenePath).AsUniTask();
        }
        
        public UniTask RemoveScene(AssetReference sceneReference)
        {
            return ManageAddressables.UnloadSceneAsync(sceneReference).AsUniTask();
        }

        public async UniTask<SceneInstance> Switch2Scene(string scenePath, Action<float> progressHandler, bool activateOnLoad = true)
        {
            // 加载场景
            var operate = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Single, activateOnLoad);
            
            // 开始跟踪进度条
            var cts = new CancellationTokenSource();
            SceneLoadProgress(operate, progressHandler,cts.Token).Forget();
            
            // 等待加载完成
            var result = await operate.Task.AsUniTask();;
            
            // 停止跟踪进度条
            cts.Cancel();
            cts.Dispose();
            
            return result;
        }

        public async UniTask<SceneInstance> Switch2Scene(AssetReference sceneReference, Action<float> progressHandler, bool activateOnLoad = true)
        {
            // 加载场景
            var operate = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Single, activateOnLoad);
            
            // 开始跟踪进度条
            var cts = new CancellationTokenSource();
            SceneLoadProgress(operate, progressHandler,cts.Token).Forget();
            
            // 等待加载完成
            var result = await operate.Task.AsUniTask();;
            
            // 停止跟踪进度条
            cts.Cancel();
            cts.Dispose();
            
            return result;
        }

        async UniTask SceneLoadProgress(AsyncOperationHandle<SceneInstance> operationHandle, Action<float> progressHandler, CancellationToken cancellationToken)
        {
            bool needProgress = progressHandler != null;
            while (operationHandle.PercentComplete < 1 && !cancellationToken.IsCancellationRequested)
            {
                if (needProgress)
                {
                    progressHandler(operationHandle.PercentComplete);
                }
                await UniTask.Yield();
            }

            if (needProgress)
            {
                progressHandler(1);
            }
        }
    }
}
