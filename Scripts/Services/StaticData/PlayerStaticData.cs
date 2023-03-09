using UnityEngine;

namespace Services.StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static Data/Player")]
    public class PlayerStaticData : ScriptableObject
    {
        [Range(1,100)]
        public int BackpackSize = 40;
    }
}