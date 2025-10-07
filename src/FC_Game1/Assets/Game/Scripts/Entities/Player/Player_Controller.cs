using FCTools.UIView;
using UnityEngine;

namespace Game
{
    public class Player_Controller : MonoBehaviour
    {
        #region inspector

        #endregion

        #region properties
        private Player m_player;
        #endregion

        public void Setup(Player a_player)
        {
            m_player = a_player;
        }
        public void Init()
        {
            m_player.Player_InputComponent.onMove += OnPlayerMoveInput;
            m_player.Player_InputComponent.onSprint += OnPlayerSprintInput;

            m_player.Player_InteractionComponent.onTriggerDelegate += OnPlayerTriggerInteractor;
            m_player.Player_InteractionComponent.onUnTriggerDelegate += OnPlayerUnTriggerInteractor;

            m_player.Player_InputComponent.onInteract += OnPlayerInteract;

            m_player.Player_InputComponent.onOpenMenu += OnPlayerOpenMenu;
            m_player.Player_InputComponent.onCloseMenu += OnPlayerCloseMenu;
        }

        public void DoOnDestroy()
        {
            m_player.Player_InputComponent.onMove -= OnPlayerMoveInput;
            m_player.Player_InputComponent.onSprint -= OnPlayerSprintInput;

            m_player.Player_InteractionComponent.onTriggerDelegate -= OnPlayerTriggerInteractor;
            m_player.Player_InteractionComponent.onUnTriggerDelegate -= OnPlayerUnTriggerInteractor;

            m_player.Player_InputComponent.onInteract -= OnPlayerInteract;

            m_player.Player_InputComponent.onOpenMenu -= OnPlayerOpenMenu;
            m_player.Player_InputComponent.onCloseMenu -= OnPlayerCloseMenu;
        }

        private void OnPlayerMoveInput(Vector3 a_moveInput)
        {
            m_player.Player_MovementComponent.Move(a_moveInput);
        }
        private void OnPlayerSprintInput(bool a_sprintInput)
        {
            m_player.Player_MovementComponent.Sprint(a_sprintInput);
        }


        private void OnPlayerTriggerInteractor(Entity a_entity, IInteractable a_interactor, Transform a_target)
        {
            GUIManager.Instance.ShowPrompt(a_interactor.GetInteractionPrompt(), a_target);
        }
        private void OnPlayerUnTriggerInteractor(Entity a_entity, IInteractable a_interactor, Transform a_target)
        {
            GUIManager.Instance.HidePrompt();
        }
        private void OnPlayerInteract(IInteractable a_target)
        {
            GUIManager.Instance.HidePrompt();
            a_target.Interact(m_player);
            m_player.Player_InteractionComponent.ClearCurrentTarget();
            // UIManager.Instance.Push<InventoryWindow>(a_target.Interact).Refresh(m_player.Inventory);
        }
        private void OnPlayerOpenMenu(string a_menuId)
        {
            if (UIManager.Instance.IsOpen(a_menuId))
            {
                OnPlayerCloseMenu();
                return;
            }
            UIManager.Instance.Push<InventoryWindow>(a_menuId).Refresh(m_player.Inventory);
        }
        private void OnPlayerCloseMenu()
        {
            UIManager.Instance.Pop();
        }
    }
}
