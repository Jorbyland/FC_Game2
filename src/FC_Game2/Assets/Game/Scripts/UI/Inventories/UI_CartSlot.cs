using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI_CartSlot : MonoBehaviour
    {
        [SerializeField] private Image m_iconImg;
        [SerializeField] private TextMeshProUGUI m_nameTMP;
        [SerializeField] private TextMeshProUGUI m_a_qtyTMP;
        [SerializeField] private TextMeshProUGUI m_a_priceTMP;
        [SerializeField] private Button m_plusBtn;
        [SerializeField] private Button m_minusBtn;
        [SerializeField] private Button m_removeBtn;

        private ItemScriptable m_currentItem;
        private int m_unitPrice;
        private System.Action<ItemScriptable,int> m_onChangeQty;
        private System.Action<ItemScriptable> m_onRemove;

        public void Setup(ItemScriptable a_item, int a_qty, int a_price,
                          System.Action<ItemScriptable,int> onQtyChanged,
                          System.Action<ItemScriptable> onRemoved)
        {
            m_currentItem = a_item;
            m_unitPrice = a_price;
            m_onChangeQty = onQtyChanged;
            m_onRemove = onRemoved;

            Refresh(a_qty);

            m_iconImg.sprite = a_item.Icon;
            m_nameTMP.text = a_item.DisplayName;

            m_plusBtn.onClick.RemoveAllListeners();
            m_plusBtn.onClick.AddListener(() => m_onChangeQty(a_item, +1));

            m_minusBtn.onClick.RemoveAllListeners();
            m_minusBtn.onClick.AddListener(() => m_onChangeQty(a_item, -1));

            m_removeBtn.onClick.RemoveAllListeners();
            m_removeBtn.onClick.AddListener(() => m_onRemove(a_item));
        }

        public void Refresh(int a_qty)
        {
            m_a_qtyTMP.text = $"x{a_qty}";
            m_a_priceTMP.text = $"{m_unitPrice * a_qty} $";
        }
    }
}