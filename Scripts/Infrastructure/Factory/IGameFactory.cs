using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
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
  }
}