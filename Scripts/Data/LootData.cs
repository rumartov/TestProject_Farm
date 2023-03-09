using System;

namespace Data
{
    [Serializable]
    public class LootData
    {
        public StackData StackData;
        public MoneyData MoneyData;

        public LootData()
        {
            StackData = new StackData();
            MoneyData = new MoneyData();
        }
    }
}