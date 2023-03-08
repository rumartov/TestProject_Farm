using Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    private IAssetProvider assetProviderImplementation;

    public GameObject Instantiate(string path, Vector3 at, Quaternion quaternion)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab, at, quaternion);
    }

    public GameObject Instantiate(string path, Vector3 at)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab, at, Quaternion.identity);
    }

    public GameObject Instantiate(string path)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab);
    }
  }
}