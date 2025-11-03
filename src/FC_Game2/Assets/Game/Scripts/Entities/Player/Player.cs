using FCTools;
using UnityEngine;

namespace Game
{
    public class Player : Actor
    {
        #region inspector
        [SerializeField] private Player_Controller m_playerController;
        public Player_Controller Player_Controller => m_playerController;
        [SerializeField] private Player_MovementComponent m_playerMovementComponent;
        public Player_MovementComponent Player_MovementComponent => m_playerMovementComponent;
        [SerializeField] private Player_InputComponent m_player_InputComponent;
        public Player_InputComponent Player_InputComponent => m_player_InputComponent;
        [SerializeField] private Player_InteractionComponent m_interactionComponent;
        public Player_InteractionComponent Player_InteractionComponent => m_interactionComponent;
        [SerializeField] private Transform m_visual;
        public Transform Visual => m_visual;

        #endregion

        #region properties
        private Inventory m_inventory;
        public Inventory Inventory => m_inventory;
        private Currency m_wallet;
        public Currency Wallet => m_wallet;
        #endregion

        public override void Setup(GameContextScriptable a_gameContext)
        {
            base.Setup(a_gameContext);
            m_inventory = new Inventory();
            m_wallet = new Currency(a_gameContext.PlayerStartMoney);
            m_playerMovementComponent.Setup(this);
            m_player_InputComponent.Setup(this);
            m_interactionComponent.Setup(this);
            m_playerController.Setup(this);
        }
        public override void Init(GameState a_gameState)
        {
            base.Init(a_gameState);
            m_inventory = new Inventory(a_gameState.PlayerInventory);
            m_wallet = new Currency(a_gameState.PlayerWallet);
            m_playerMovementComponent.Init();
            m_player_InputComponent.Init();
            m_interactionComponent.Init();
            m_playerController.Init();
        }

        protected void Update()
        {
            // m_player_InputComponent.DoUpdate();
            m_interactionComponent.DoUpdate();
            m_playerMovementComponent.DoUpdate();
        }

        protected void OnDestroy()
        {
            m_player_InputComponent.DoOnDestroy();
            m_playerController.DoOnDestroy();
            m_interactionComponent.DoOnDestroy();
        }
    }
}
