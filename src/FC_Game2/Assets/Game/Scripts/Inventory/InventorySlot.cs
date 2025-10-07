using UnityEngine;

namespace Game
{
    public class InventorySlot
    {
        public ItemScriptable Item;
        public int Quantity;

        public InventorySlot(ItemScriptable a_item, int a_qty)
        {
            Item = a_item;
            Quantity = a_qty;
        }
    }
}
