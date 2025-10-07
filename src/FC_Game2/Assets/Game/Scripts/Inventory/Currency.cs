using UnityEngine;

namespace Game
{
    public class Currency
    {
        public event System.Action OnChange;
        #region properties
        public int Balance { get; private set; }
        #endregion

        public Currency(int start)
        {
            Balance = start;
            OnChange?.Invoke();
        }


        public bool CanAfford(int price) => Balance >= price;

        public void Add(int value)
        {
            Balance += value;
            OnChange?.Invoke();
        }

        public bool Spend(int value)
        {
            if (Balance < value) return false;
            Balance -= value;
            OnChange?.Invoke();
            return true;
        }
    }
}
