using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
    /// <summary>
    /// ���ҽ��� �񵿱� �ε��մϴ�.
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
    /// �ݹ� �ε�
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
    /// ������Ʈ�� �񵿱� �ν��Ͻ�ȭ�մϴ�.
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
    /// ���ҽ��� �����մϴ� (������ó�� LoadAssetAsync�� �ε�� ���ҽ�).
    /// </summary>
    public void Release<T>(T obj) where T : UnityEngine.Object
    {
        Addressables.Release(obj);
    }

    /// <summary>
    /// �ν��Ͻ��� ������Ʈ�� �����մϴ� (InstantiateAsync�� ������ ���).
    /// </summary>
    public void ReleaseInstance(GameObject instance)
    {
        Addressables.ReleaseInstance(instance);
    }
}
