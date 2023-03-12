using Data;
using TMPro;
using UnityEngine;

public class StackCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counter;
    private WorldData _worldData;

    public void Construct(WorldData worldData)
    {
        _worldData = worldData;
        _worldData.LootData.StackData.Changed += UpdateCounter;
    }

    private void UpdateCounter()
    {
        counter.text = $"{_worldData.LootData.StackData.Collected}";
    }
}