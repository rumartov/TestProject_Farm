using CameraLogic;
using Cinemachine;
using Infrastructure.Factory;
using Logic;
using Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string PlayerSpawnPosition = "PlayerSpawnPosition";
    private const string BarnSpawnPosition = "BarnSpawnPosition";
    private const string GardenSpawnPosition = "GardenSpawnPosition";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _gameFactory.Cleanup();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _loadingCurtain.Hide();

    private void OnLoaded()
    {
      InitGameWorld();
      InformProgressReaders();

      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitGameWorld()
    {
      GameObject playerSpawnPosition = GameObject.FindWithTag(PlayerSpawnPosition);
      GameObject barnSpawnPosition = GameObject.FindWithTag(BarnSpawnPosition);
      GameObject gardenSpawnPosition = GameObject.FindWithTag(GardenSpawnPosition);
      GameObject player = _gameFactory.CreatePlayer(playerSpawnPosition);
      _gameFactory.CreateBarn(barnSpawnPosition);
      _gameFactory.CreateGarden(gardenSpawnPosition);
      _gameFactory.CreateHud();

      CameraFollow(player);
    }

    private void CameraFollow(GameObject target)
    {
      CinemachineVirtualCamera virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
      virtualCamera.Follow = target.transform;
      virtualCamera.LookAt = target.transform;
    }
  }
}