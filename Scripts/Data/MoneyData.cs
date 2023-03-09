using System;

namespace Data
{
    [Serializable]
    public class MoneyData
    {
        public Action Changed;
        public int Collected;
        
        public void Add(int value)
        {
            Collected += value;
            Changed?.Invoke();
        }
        
        public void Remove(int value)
        {
            Collected -= value;
            Changed?.Invoke();
        }
    }
}