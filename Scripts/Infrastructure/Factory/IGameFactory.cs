using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory:IService
  {
    GameObject CreatePlayer(GameObject at);
    void CreateHud();
    List<ISavedProgressReader> ProgressReaders { get; }
    GameObject PlayerGameObject { get; }
    event Action PlayerCreated; 
    List<ISavedProgress> ProgressWriters { get; }
    void Cleanup();
    GameObject CreatePlant(VegetationType vegetationType, Vector3 at, Transform parent);
    void CreateGarden(GameObject at);
    string GetVegetationAssetPath(VegetationType vegetationType);
  }
}