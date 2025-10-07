using UnityEngine;

namespace Game
{
    public abstract class Item
    {
        #region properties
        public string Name { get; }
        public string Description { get; }
        public int MaxStack { get; }
        public int BasePrice { get; }
        #endregion


        protected Item(string a_name, string a_description, int a_basePrice, int a_maxStack = 1)
        {
            Name = a_name;
            Description = a_description;
            BasePrice = a_basePrice;
            MaxStack = a_maxStack;
        }
    }
}
