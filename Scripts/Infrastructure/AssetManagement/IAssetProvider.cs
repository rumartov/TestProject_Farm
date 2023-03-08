using CodeBase.Services;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
  public interface IAssetProvider:IService
  {
    GameObject Instantiate(string path, Vector3 at, Quaternion quaternion);
    GameObject Instantiate(string path, Vector3 at);
    GameObject Instantiate(string path);
  }
}