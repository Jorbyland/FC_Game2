using System.Linq;
using FCTools.UIView;
using UnityEngine;

namespace Game
{
    public class VendorNpc : Npc
    {
        #region properties
        private VendorInventory m_shopInventory;
        public VendorInventory ShopInventory => m_shopInventory;
        #endregion

        public override void Setup(GameContextScriptable a_context)
        {
            base.Setup(a_context);
            gameObject.name = Id;
            m_shopInventory = new VendorInventory();

            NpcVendorScriptable npcVendorSO = a_context.GetNpcVendorScriptable(Id);
            if (npcVendorSO != null)
            {
                foreach (var entry in npcVendorSO.StartingStock)
                {
                    m_shopInventory.SetItem(entry.Item, entry.DefaultQuantity);
                    int price = entry.OverridePrice > 0 ? entry.OverridePrice : entry.Item.BasePrice;
                    m_shopInventory.SetPrice(entry.Item, price);
                }
            }
            else
            {
                Debug.LogError($"Cannot find NpcVendorScriptable with id : {Id}");
            }
        }

        public override void Init(GameState a_state)
        {
            base.Init(a_state);
            if (a_state.NpcVendors.TryGetValue(Id, out var vendorData) && vendorData.Slots.Count > 0)
            {
                m_shopInventory = new VendorInventory();
                foreach (var slot in vendorData.Slots)
                {
                    m_shopInventory.SetItem(slot.ItemSO, slot.Quantity);
                    m_shopInventory.SetPrice(slot.ItemSO, slot.Price);
                }
            }
        }
        public override void Interact(Entity a_interactor)
        {
            UIManager.Instance.Push<VendorWindow>("vendor_shop").OpenShop(m_shopInventory, a_interactor.GetComponent<Player>());
        }
    }
}
