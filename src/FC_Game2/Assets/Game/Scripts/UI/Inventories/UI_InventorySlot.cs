using FCTools.UIView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI_InventorySlot : MonoBehaviour
    {
        #region properties
        [SerializeField] private Image m_iconImg;
        [SerializeField] private TextMeshProUGUI m_nameTMP;
        [SerializeField] private TextMeshProUGUI m_descriptionTMP;
        [SerializeField] private TextMeshProUGUI m_priceTMP;
        [SerializeField] private TextMeshProUGUI m_quantityTMP;
        [SerializeField] private Button m_addBtn;

        private ItemScriptable m_currentItem;
        private int m_currentPrice;
        private System.Action<ItemScriptable> m_onAddToCart;
        #endregion


        public void SetShopItem(ItemScriptable a_item, int a_price, System.Action<ItemScriptable> a_onAdd)
        {
            m_currentItem = a_item;
            m_currentPrice = a_price;
            m_onAddToCart = a_onAdd;

            m_iconImg.sprite = a_item.Icon;
            m_nameTMP.text = a_item.DisplayName;
            m_descriptionTMP.text = a_item.Description;
            m_priceTMP.text = $"{a_price} $";
            m_quantityTMP.gameObject.SetActive(false);

            m_addBtn.onClick.RemoveAllListeners();
            m_addBtn.onClick.AddListener(() => m_onAddToCart?.Invoke(a_item));
        }

        public void SetPlayerItem(ItemScriptable a_item, int a_quantity)
        {
            m_currentItem = a_item;
            m_iconImg.sprite = a_item.Icon;
            m_nameTMP.text = a_item.DisplayName;
            m_descriptionTMP.text = a_item.Description;
            m_quantityTMP.text = $"{a_quantity}";
            m_quantityTMP.gameObject.SetActive(true);
            m_priceTMP.gameObject.SetActive(false);
            m_addBtn.gameObject.SetActive(false);
        }
    }
}
