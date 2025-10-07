using UnityEngine;

namespace Game
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager Instance { get; private set; }

        [SerializeField] private GUIWallet m_wallet;
        [SerializeField] private GUIPrompts m_prompts;

        private Player m_player;

        private void Awake()
        {
            Instance = this;
        }

        public void Init(Player a_player, GameContextScriptable a_context, GameState a_state)
        {
            m_player = a_player;

            m_wallet.Bind(m_player.Wallet);
            m_prompts.Clear();
        }

        public void ShowPrompt(string a_message, Transform a_target) => m_prompts.Show(a_message, a_target);
        public void HidePrompt() => m_prompts.Hide();
    }
}
