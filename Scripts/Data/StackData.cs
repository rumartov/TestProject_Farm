using System;

namespace Data
{
    [Serializable]
    public class StackData
    {
        public int Collected;
        public Action Changed;

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