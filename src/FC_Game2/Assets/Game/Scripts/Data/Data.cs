using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class InventorySlotData
    {
        public string ItemId;
        public int Quantity;
        public int Price; // utilisé seulement pour Vendor
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<InventorySlotData> Slots = new List<InventorySlotData>();
    }


}
