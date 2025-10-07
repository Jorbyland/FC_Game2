using TMPro;
using UnityEngine;

namespace Game
{
    public class GUIWallet : GUIPanel
    {
        [SerializeField] private TextMeshProUGUI m_moneyTMP;
        private Currency m_currency;
        public override void Bind(GameContextScriptable context, GameState state)
        {
        }
        public void Bind(Currency a_currency)
        {
            m_currency = a_currency;
            m_currency.OnChange += Refresh;
            Refresh();
        }

        public override void Refresh()
        {
            m_moneyTMP.text = $"{m_currency.Balance}$";
        }
    }
}
