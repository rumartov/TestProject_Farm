using CodeBase.Services;
using StaticData;

namespace Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    PlantStaticData ForPlant(VegetationType typeId);
    PlayerStaticData ForPlayer();
  }
}