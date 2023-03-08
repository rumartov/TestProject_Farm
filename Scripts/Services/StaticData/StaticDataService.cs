using System.Collections.Generic;
using System.Linq;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string PlantsDataPath = "StaticData/Vegetation";

    private Dictionary<VegetationType, PlantStaticData> _plants;
    
    public void Load()
    {
      _plants = Resources
        .LoadAll<PlantStaticData>(PlantsDataPath)
        .ToDictionary(x => x.VegetationType, x => x);
    }

    public PlantStaticData ForPlant(VegetationType typeId) =>
      _plants.TryGetValue(typeId, out PlantStaticData staticData)
        ? staticData
        : null;
  }
}