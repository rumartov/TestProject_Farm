using System.Collections.Generic;
using System.Linq;
using Logic.Vegetation;
using StaticData;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string VegetationDataPath = "StaticData/Vegetation";
        private const string PlayerDataPath = "StaticData/Player";

        private Dictionary<VegetationType, PlantStaticData> _plants;
        private PlayerStaticData _player;

        public void Load()
        {
            _plants = Resources
                .LoadAll<PlantStaticData>(VegetationDataPath)
                .ToDictionary(x => x.VegetationType, x => x);
            _player = Resources
                .LoadAll<PlayerStaticData>(PlayerDataPath)
                .First();
        }

        public PlayerStaticData ForPlayer()
        {
            return _player;
        }

        public PlantStaticData ForPlant(VegetationType typeId)
        {
            return _plants.TryGetValue(typeId, out var staticData)
                ? staticData
                : null;
        }
    }
}