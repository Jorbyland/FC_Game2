using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Config/NpcVendorDefinition")]
    public class NpcVendorScriptable : ScriptableObject
    {
        [System.Serializable]
        public class StockEntry
        {
            public ItemScriptable Item;
            public int DefaultQuantity;
            public int OverridePrice = -1; // si -1 → utiliser Item.BasePrice
        }
        public string Id;
        public List<StockEntry> StartingStock = new List<StockEntry>();
    }
}
