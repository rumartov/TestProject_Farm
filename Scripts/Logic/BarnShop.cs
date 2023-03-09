using Logic;
using Services.PersistentProgress;
using UnityEngine;

public class BarnShop : MonoBehaviour
{
    private IPersistentProgressService _progressService;

    public void Construct(IPersistentProgressService progressService)
    {
        _progressService = progressService;
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
        _progressService.Progress.WorldData.LootData.MoneyData.Add(backpack.Container.Count);
        backpack.RemoveAllItems();
    }
}
