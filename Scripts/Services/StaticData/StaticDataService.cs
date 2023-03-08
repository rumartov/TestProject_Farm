using System.Collections.Generic;
using System.Linq;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace CodeBase.Services.StaticData
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

    public PlayerStaticData ForPlayer() =>
      _player;
    public PlantStaticData ForPlant(VegetationType typeId) =>
      _plants.TryGetValue(typeId, out PlantStaticData staticData)
        ? staticData
        : null;
        
  }
}