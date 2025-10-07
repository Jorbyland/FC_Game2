using System.Collections.Generic;
using FCTools.UIView;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InventoryWindow : UIWindow
    {
        #region inspector
        [SerializeField] private UI_InventorySlot m_inventorySlotTemplate;
        [SerializeField] private RectTransform m_content;
        [SerializeField] private Button m_closeBtn;
        #endregion

        #region properties
        private List<UI_InventorySlot> m_slots = new List<UI_InventorySlot>();

        #endregion
        public void Refresh(Inventory a_inventory)
        {
            // Met à jour les slots UI depuis l'inventaire
            m_inventorySlotTemplate.gameObject.SetActive(false);

            m_closeBtn.onClick.RemoveAllListeners();
            m_closeBtn.onClick.AddListener(() => UIManager.Instance.Pop());

            PrepareSlots(a_inventory.Slots.Count);
            FillSlots(a_inventory);
        }

        private void FillSlots(Inventory a_inventory)
        {
            for (int i = 0; i < a_inventory.Slots.Count; i++)
            {
                m_slots[i].SetPlayerItem(a_inventory.Slots[i].Item, a_inventory.Slots[i].Quantity);
                m_slots[i].gameObject.SetActive(true);
            }
        }

        private void PrepareSlots(int a_amount)
        {
            DisableSlots();
            if (m_slots.Count < a_amount)
            {
                int currentSlotCount = m_slots.Count;
                for (int i = 0; i < a_amount - currentSlotCount; i++)
                {
                    CreateSlot();
                }
            }
        }

        private void DisableSlots()
        {
            foreach (var slot in m_slots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        private UI_InventorySlot CreateSlot()
        {
            UI_InventorySlot instance = Instantiate(m_inventorySlotTemplate.gameObject).GetComponent<UI_InventorySlot>();
            instance.gameObject.transform.SetParent(m_content);
            instance.gameObject.SetActive(true);
            m_slots.Add(instance);
            return instance;
        }
    }
}
