using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Inventory
    {
        protected List<InventorySlot> slots = new List<InventorySlot>();

        public IReadOnlyList<InventorySlot> Slots => slots;

        public Inventory()
        {
            slots = new List<InventorySlot>();
        }
        public Inventory(InventoryState a_inventoryState)
        {
            slots = new List<InventorySlot>();
            if (a_inventoryState != null)
            {
                foreach (var item in a_inventoryState.Slots)
                {
                    SetItem(item.ItemSO, item.Quantity);
                }
            }
        }

        public bool SetItem(ItemScriptable a_item, int a_qty)
        {
            var slot = slots.FirstOrDefault(s => s.Item == a_item);
            if (slot != null)
                slot.Quantity = a_qty;
            else
                slots.Add(new InventorySlot(a_item, a_qty));
            return true;
        }

        public bool AddItem(ItemScriptable a_item, int a_qty)
        {
            var slot = slots.FirstOrDefault(s => s.Item == a_item);
            if (slot != null)
                slot.Quantity += a_qty;
            else
                slots.Add(new InventorySlot(a_item, a_qty));
            return true;
        }

        public bool RemoveItem(ItemScriptable a_item, int a_qty)
        {
            var slot = slots.FirstOrDefault(s => s.Item == a_item);
            if (slot == null || slot.Quantity < a_qty)
                return false;

            slot.Quantity -= a_qty;
            if (slot.Quantity <= 0)
                slots.Remove(slot);
            return true;
        }

        public bool HasItem(ItemScriptable a_item, int a_qty = 1) =>
            slots.Any(s => s.Item == a_item && s.Quantity >= a_qty);
    }
}
