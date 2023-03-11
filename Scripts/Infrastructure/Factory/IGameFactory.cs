using System;
using System.Collections.Generic;
using Logic.Vegetation;
using Services;
using Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Factory
{
  public interface IGameFactory:IService
  {
    GameObject CreatePlayer(GameObject at);
    void CreateHud();
    List<ISavedProgressReader> ProgressReaders { get; }
    GameObject PlayerGameObject { get; }
    event Action PlayerCreated; 
    List<ISavedProgress> ProgressWriters { get; }
    GameObject Hud { get; set; }
    void Cleanup();
    GameObject CreatePlant(VegetationType vegetationType, Vector3 at, Transform parent);
    void CreateGarden(GameObject at);
    void CreateBarn(GameObject barnSpawnPosition);
    void CreateHarvest(VegetationType vegetationType, Vector3 at, Transform parent = null);
    GameObject CreateCoinIcon(GameObject at);
  }
}