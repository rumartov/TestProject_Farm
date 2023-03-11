using Logic.Vegetation;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PlantData", menuName = "Static Data/Plant")]
    public class PlantStaticData : ScriptableObject
    {
        public VegetationType VegetationType;

        [Range(1,100)]
        public float GrowTime = 10;

        [Range(1, 100)] 
        public int SellCost = 1;

        [Range(1,100)]
        public float HarvestAmount = 10;

        public  GameObject HarvestPrefab;
    }
}