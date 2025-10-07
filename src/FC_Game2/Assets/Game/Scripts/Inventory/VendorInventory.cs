using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class VendorInventory : Inventory
    {
        private Dictionary<ItemScriptable, int> m_prices = new Dictionary<ItemScriptable, int>();

        public void SetPrice(ItemScriptable a_item, int a_price)
        {
            m_prices[a_item] = a_price;
        }

        public int GetPrice(ItemScriptable a_item)
        {
            if (m_prices.TryGetValue(a_item, out var p))
                return p;
            return a_item.BasePrice;
        }
    }
}
