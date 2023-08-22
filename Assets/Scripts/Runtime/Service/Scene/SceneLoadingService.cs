using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public static class SceneLoadingService
{
    public static AssetReference SceneToLoad { get; set; }
    public static AsyncOperationHandle<SceneInstance> LoadScene(AssetReference sceneReference, LoadSceneMode loadMode = LoadSceneMode.Single,
        Action<AsyncOperationHandle<SceneInstance>> callback = null)
    {
        Debug.Log($"Loading: {sceneReference.SubObjectName}");
        AsyncOperationHandle<SceneInstance> loadHandle = Addressables.LoadSceneAsync(sceneReference, loadMode, activateOnLoad: true);
        
        if(callback != null)
            loadHandle.Completed += callback;

        return loadHandle;
    }
}
