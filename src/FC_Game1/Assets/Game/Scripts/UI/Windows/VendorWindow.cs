using System.Collections.Generic;
using FCTools.UIView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class VendorWindow : UIWindow
    {
        [Header("Shop")]
        [SerializeField] private UI_InventorySlot m_a_inventorySlotTemplate;
        [SerializeField] private RectTransform m_shopContent;

        [Header("Cart")]
        [SerializeField] private UI_CartSlot m_cartSlotTemplate;
        [SerializeField] private RectTransform m_cartContent;
        [SerializeField] private TextMeshProUGUI m_cartTotalTMP;
        [SerializeField] private Button m_buyBtn;

        [Header("Controls")]
        [SerializeField] private Button m_closeBtn;

        private List<UI_InventorySlot> m_shopSlots = new();
        private List<UI_CartSlot> m_cartSlots = new();

        private ShoppingCart m_cart = new ShoppingCart();
        private VendorInventory m_currentInventory;
        private Player m_currentPlayer;

        public void OpenShop(VendorInventory a_inventory, Player a_player)
        {
            m_currentInventory = a_inventory;
            m_currentPlayer = a_player;
            m_cart.Clear();

            m_cartSlotTemplate.gameObject.SetActive(false);
            m_a_inventorySlotTemplate.gameObject.SetActive(false);

            m_closeBtn.onClick.RemoveAllListeners();
            m_closeBtn.onClick.AddListener(() => UIManager.Instance.Pop());

            m_buyBtn.onClick.RemoveAllListeners();
            m_buyBtn.onClick.AddListener(OnBuyCart);

            m_cart.OnCartChanged += RefreshCart;

            RefreshShop(a_inventory);
            RefreshCart();
        }

        private void RefreshShop(VendorInventory a_inventory)
        {
            PrepareShopSlots(a_inventory.Slots.Count);

            for (int i = 0; i < a_inventory.Slots.Count; i++)
            {
                var slot = a_inventory.Slots[i];
                int price = GetFinalPrice(slot.Item, a_inventory.GetPrice(slot.Item));
                m_shopSlots[i].SetShopItem(slot.Item, price, item => m_cart.Add(item));
                m_shopSlots[i].gameObject.SetActive(true);
            }
        }

        private void RefreshCart()
        {
            foreach (var slot in m_cartSlots) slot.gameObject.SetActive(false);

            int index = 0;
            int total = 0;

            foreach (var kvp in m_cart.Items)
            {
                int unitPrice = GetFinalPrice(kvp.Key, m_currentInventory.GetPrice(kvp.Key));
                total += unitPrice * kvp.Value;

                if (index >= m_cartSlots.Count)
                    m_cartSlots.Add(CreateCartSlot());

                var uiSlot = m_cartSlots[index];
                uiSlot.Setup(
                    kvp.Key, kvp.Value, unitPrice,
                    (item, delta) => m_cart.SetQuantity(item, m_cart.GetQuantity(item) + delta),
                    item => m_cart.Remove(item)
                );
                uiSlot.gameObject.SetActive(true);
                index++;
            }

            m_cartTotalTMP.text = $"Total: {total} $";
        }

        private void OnBuyCart()
        {
            int total = 0;
            foreach (var kvp in m_cart.Items)
                total += GetFinalPrice(kvp.Key, m_currentInventory.GetPrice(kvp.Key)) * kvp.Value;

            if (!m_currentPlayer.Wallet.CanAfford(total))
            {
                Debug.Log("Player haven't enought money to buy this cart");
                return;
            }

            foreach (var kvp in m_cart.Items)
            {
                if (m_currentInventory.HasItem(kvp.Key, kvp.Value))
                {
                    if (m_currentPlayer.Inventory.AddItem(kvp.Key, kvp.Value))
                        m_currentInventory.RemoveItem(kvp.Key, kvp.Value);
                }
            }

            m_currentPlayer.Wallet.Spend(total);
            m_cart.Clear();

            RefreshShop(m_currentInventory);
        }

        private int GetFinalPrice(ItemScriptable item, int basePrice)
        {
            if (item.Id == "Potion")
                return Mathf.RoundToInt(basePrice * 0.5f);
            return basePrice;
        }

        private void PrepareShopSlots(int count)
        {
            foreach (var slot in m_shopSlots) slot.gameObject.SetActive(false);
            while (m_shopSlots.Count < count) m_shopSlots.Add(CreateShopSlot());
        }

        private UI_InventorySlot CreateShopSlot()
        {
            var instance = Instantiate(m_a_inventorySlotTemplate, m_shopContent);
            instance.gameObject.SetActive(true);
            return instance;
        }

        private UI_CartSlot CreateCartSlot()
        {
            var instance = Instantiate(m_cartSlotTemplate, m_cartContent);
            instance.gameObject.SetActive(true);
            return instance;
        }
    }
}
