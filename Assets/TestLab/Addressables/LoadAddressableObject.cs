using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAddressableObject : MonoBehaviour
{
    private async void Start()
    {
        GameObject cube = await LoadAddressable<GameObject>("Cube");
        cube.transform.position = Vector3.zero;
    }

    private static async Task<T> LoadAddressable<T>(string key) where T : Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            T instance = Object.Instantiate(handle.Result);
            return instance;
        }
        return default;
    }
}
