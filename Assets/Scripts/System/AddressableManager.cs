using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
    /// <summary>
    /// 리소스를 비동기 로드합니다.
    /// </summary>
    public async Task<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }

        Debug.LogError($"[AddressableManager] Failed to load asset: {path}");
        return null;
    }

    /// <summary>
    /// 콜백 로드
    /// </summary>
    public void LoadAsset<T>(string path, Action<T> onLoaded) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                onLoaded?.Invoke(op.Result);
            }
            else
            {
                Debug.LogError($"[AddressableManager] Failed to load asset (callback): {path}");
                onLoaded?.Invoke(null);
            }
        };
    }

    /// <summary>
    /// 오브젝트를 비동기 인스턴스화합니다.
    /// </summary>
    public async Task<GameObject> InstantiateAsync(string path, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        var handle = Addressables.InstantiateAsync(path, position, rotation, parent);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }

        Debug.LogError($"[AddressableManager] Failed to instantiate: {path}");
        return null;
    }

    /// <summary>
    /// 리소스를 해제합니다 (프리팹처럼 LoadAssetAsync로 로드된 리소스).
    /// </summary>
    public void Release<T>(T obj) where T : UnityEngine.Object
    {
        Addressables.Release(obj);
    }

    /// <summary>
    /// 인스턴스된 오브젝트를 해제합니다 (InstantiateAsync로 생성된 경우).
    /// </summary>
    public void ReleaseInstance(GameObject instance)
    {
        Addressables.ReleaseInstance(instance);
    }
}
