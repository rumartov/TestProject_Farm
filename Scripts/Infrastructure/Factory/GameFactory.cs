using System;
using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Logic;
using Logic.Vegetation;
using Services.Input;
using Services.PersistentProgress;
using Services.Randomizer;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _progressService;
        private readonly IRandomService _randomService;
        private readonly IStaticDataService _staticDataService;

        public GameFactory(IAssetProvider assets, IRandomService randomService, IStaticDataService staticDataService,
            IInputService inputService, IPersistentProgressService progressService)
        {
            _assets = assets;
            _randomService = randomService;
            _staticDataService = staticDataService;
            _inputService = inputService;
            _progressService = progressService;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public GameObject PlayerGameObject { get; set; }
        public GameObject Hud { get; set; }

        public event Action PlayerCreated;

        public GameObject CreatePlayer(GameObject at)
        {
            PlayerGameObject = InstantiateRegistered(AssetPath.PlayerPath, at.transform.position);
            var backpack = PlayerGameObject.GetComponentInChildren<Backpack>();
            backpack.Construct(_staticDataService.ForPlayer().BackpackSize, _inputService, _progressService);
            PlayerCreated?.Invoke();
            return PlayerGameObject;
        }

        public void CreateGarden(GameObject at)
        {
            var garden = InstantiateRegistered(AssetPath.Garden, at.transform.position);
            garden.GetComponent<Garden>().Construct(VegetationType.Wheat, this, _staticDataService);
        }

        public void CreateBarn(GameObject at)
        {
            var barn = InstantiateRegistered(AssetPath.Barn, at.transform.position, Quaternion.Euler(0, -90, 0));

            barn.GetComponentInChildren<BarnShop>().Construct(_progressService, _staticDataService, this);
        }

        public GameObject CreateCoinIcon(GameObject at)
        {
            return InstantiateRegistered(AssetPath.CoinIcon, at.transform.position);
        }

        public GameObject CreatePlant(VegetationType vegetationType, Vector3 at, Transform parent)
        {
            var assetPath = GetVegetationAssetPath(vegetationType);
            float y = _randomService.Next(0, 360);
            var quaternion = Quaternion.Euler(0, y, 0);
            return InstantiateRegistered(assetPath, at, quaternion, parent);
        }

        public void CreateHarvest(VegetationType vegetationType, Vector3 at, Transform parent = null)
        {
            var harvestPath = GetHarvestAssetPath(vegetationType);
            var harvest = InstantiateRegistered(harvestPath, at, parent);
            harvest.GetComponentInChildren<Harvest>().Construct(vegetationType);
        }

        public void CreateHud()
        {
            var hud = InstantiateRegistered(AssetPath.HudPath);

            hud.GetComponentInChildren<StackCounter>()
                .Construct(_progressService.Progress.WorldData);
            hud.GetComponentInChildren<MoneyCounter>()
                .Construct(_progressService.Progress.WorldData);

            var canvas = hud.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 1;

            Hud = hud;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public GameObject CreateStack(VegetationType vegetationType, Vector3 at, Transform parent = null)
        {
            var stackPath = GetStackAssetPath(vegetationType);
            var stack = InstantiateRegistered(stackPath, at, parent);
            stack.GetComponentInChildren<Harvest>().Construct(vegetationType);

            return stack;
        }

        public string GetStackAssetPath(VegetationType vegetationType)
        {
            var enumName = vegetationType.ConvertToString();
            var assetPath = $"{AssetPath.Harvest}{enumName}Stack";
            return assetPath;
        }

        public string GetVegetationAssetPath(VegetationType vegetationType)
        {
            var enumName = vegetationType.ConvertToString();
            var assetPath = $"{AssetPath.Vegetation}{enumName}/{enumName}";
            return assetPath;
        }

        public string GetHarvestAssetPath(VegetationType vegetationType)
        {
            var enumName = vegetationType.ConvertToString();
            var assetPath = $"{AssetPath.Harvest}{enumName}Harvest";
            return assetPath;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at, Quaternion quaternion,
            Transform parent = null)
        {
            var gameObject = _assets.Instantiate(prefabPath, at, quaternion);

            SetParentTransform(parent, gameObject);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at,
            Transform parent = null)
        {
            var gameObject = _assets.Instantiate(prefabPath, at);

            SetParentTransform(parent, gameObject);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void SetParentTransform(Transform parent, GameObject gameObject)
        {
            if (parent != null) gameObject.transform.SetParent(parent);
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assets.Instantiate(prefabPath);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}