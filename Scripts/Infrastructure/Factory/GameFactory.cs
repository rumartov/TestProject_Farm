using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using DefaultNamespace;
using Infrastructure.AssetManagement;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssetProvider _assets;
    private readonly IRandomService _randomService;
    private readonly IStaticDataService _staticDataService;

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    
    public GameObject PlayerGameObject { get; set; }
    public event Action PlayerCreated;

    public GameFactory(IAssetProvider assets, IRandomService randomService, IStaticDataService staticDataService)
    {
      _assets = assets;
      _randomService = randomService;
      _staticDataService = staticDataService;
    }

    public GameObject CreatePlayer(GameObject at)
    {
      PlayerGameObject = InstantiateRegistered(AssetPath.PlayerPath, at.transform.position);
      PlayerCreated?.Invoke();
      return PlayerGameObject;
    }

    public void CreateGarden(GameObject at)
    { 
      GameObject garden = InstantiateRegistered(AssetPath.Garden, at.transform.position);
      garden.GetComponent<Garden>().Construct(VegetationType.Wheat, this, _staticDataService);
    }

    public GameObject CreatePlant(VegetationType vegetationType, Vector3 at, Transform parent)
    {
      var assetPath = GetVegetationAssetPath(vegetationType);
      float y = _randomService.Next(0, 360);
      Quaternion quaternion = Quaternion.Euler(0, y, 0);
      return InstantiateRegistered(assetPath, at, quaternion, parent);
    }

    public string GetVegetationAssetPath(VegetationType vegetationType)
    {
      string enumName = vegetationType.ConvertToString();
      string assetPath = $"{AssetPath.Vegetation}{enumName}";
      return assetPath;
    }

    public void CreateHud() =>
      InstantiateRegistered(AssetPath.HudPath);

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at, Quaternion quaternion,
      Transform parent = null)
    {
      GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at, quaternion);

      SetParentTransform(parent, gameObject);
      
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at,
      Transform parent = null)
    {
      GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at);

      SetParentTransform(parent, gameObject);
      
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }
    
    private void SetParentTransform(Transform parent, GameObject gameObject)
    {
      if (parent != null)
      {
        gameObject.transform.SetParent(parent);
      }
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
      GameObject gameObject = _assets.Instantiate(path: prefabPath);

      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
      {
        Register(progressReader);
      }
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if(progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);
      
      ProgressReaders.Add(progressReader);
    }
  }
}