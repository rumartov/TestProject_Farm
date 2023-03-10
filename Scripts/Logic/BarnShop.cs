using Logic;
using Logic.Vegetation;
using Services.PersistentProgress;
using Services.StaticData;
using UnityEngine;

public class BarnShop : MonoBehaviour
{
    private IPersistentProgressService _progressService;
    private IStaticDataService _staticData;

    public void Construct(IPersistentProgressService progressService, IStaticDataService staticData)
    {
        _progressService = progressService;
        _staticData = staticData;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SellStacks(other);
        }
    }

    private void SellStacks(Collider other)
    {
        Backpack backpack = other.GetComponentInChildren<Backpack>();
        
        foreach (GameObject item in backpack.Container)
        {
            VegetationType plantType = item.GetComponent<Harvest>().VegetationType;
            SellItem(plantType);
        }
        
        backpack.UnPackAllItems(transform);
    }

    private void SellItem(VegetationType plantType)
    {
        _progressService.Progress.WorldData.LootData.MoneyData.Add(_staticData.ForPlant(plantType).SellCost);
    }
}
