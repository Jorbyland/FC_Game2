using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player_InputComponent : MonoBehaviour
    {
        public delegate void OnMoveDelegate(Vector3 a_moveInput);
        public delegate void OnSprintDelegate(bool a_sprintInput);
        public delegate void OnInteractDelegate(IInteractable a_interactor);
        public delegate void OnOpenMenuDelegate(string a_menuId);
        public delegate void OnCloseMenuDelegate();

        // delegates exposés
        public OnMoveDelegate onMove;
        public OnSprintDelegate onSprint;
        public OnInteractDelegate onInteract;
        public OnOpenMenuDelegate onOpenMenu;
        public OnCloseMenuDelegate onCloseMenu;

        private Player m_player;
        private PlayerInput m_playerInput;
        private Vector3 m_moveInput;

        // références InputAction
        private InputAction m_moveAction;
        private InputAction m_sprintAction;
        private InputAction m_interactAction;
        private InputAction m_openInventoryAction;
        private InputAction m_closeAction;

        public void Setup(Player a_player)
        {
            m_player = a_player;
            m_playerInput = GetComponent<PlayerInput>(); // safe : RequireComponent assure la présence
        }

        // Init optionnel : peut rester vide si OnEnable gère les abonnements
        public void Init() { }

        private void OnEnable()
        {
            if (m_playerInput == null)
                m_playerInput = GetComponent<PlayerInput>();

            BindActions();
        }

        private void OnDisable()
        {
            UnbindActions();
        }

        private void BindActions()
        {
            if (m_playerInput == null)
            {
                Debug.LogError("PlayerInput manquant sur le GameObject. Assigne-le dans l'inspecteur.");
                return;
            }

            var actionsAsset = m_playerInput.actions;
            if (actionsAsset == null)
            {
                Debug.LogError("Aucun InputActions asset assigné dans PlayerInput.");
                return;
            }

            m_moveAction = actionsAsset.FindAction("Move", true);
            m_sprintAction = actionsAsset.FindAction("Sprint", true);
            m_interactAction = actionsAsset.FindAction("Interact", true);
            m_openInventoryAction = actionsAsset.FindAction("OpenInventory", true);

            // escape / close peut porter un autre nom dans ton asset : adapte si besoin
            m_closeAction = actionsAsset.FindAction("Escape", false) ?? actionsAsset.FindAction("Close", false);

            if (m_moveAction != null)
            {
                m_moveAction.performed += OnMovePerformed;
                m_moveAction.canceled += OnMoveCanceled;
            }

            if (m_sprintAction != null)
            {
                m_sprintAction.performed += OnSprintPerformed;
                m_sprintAction.canceled += OnSprintCanceled;
            }

            if (m_interactAction != null)
                m_interactAction.performed += OnInteractPerformed;

            if (m_openInventoryAction != null)
                m_openInventoryAction.performed += ctx => onOpenMenu?.Invoke("PlayerInventory");

            if (m_closeAction != null)
                m_closeAction.performed += ctx => onCloseMenu?.Invoke();
        }

        private void UnbindActions()
        {
            if (m_moveAction != null)
            {
                m_moveAction.performed -= OnMovePerformed;
                m_moveAction.canceled -= OnMoveCanceled;
                m_moveAction = null;
            }

            if (m_sprintAction != null)
            {
                m_sprintAction.performed -= OnSprintPerformed;
                m_sprintAction.canceled -= OnSprintCanceled;
                m_sprintAction = null;
            }

            if (m_interactAction != null)
            {
                m_interactAction.performed -= OnInteractPerformed;
                m_interactAction = null;
            }

            if (m_openInventoryAction != null)
            {
                m_openInventoryAction.performed -= ctx => onOpenMenu?.Invoke("PlayerInventory"); // no-op removal safe
                m_openInventoryAction = null;
            }

            if (m_closeAction != null)
            {
                m_closeAction.performed -= ctx => onCloseMenu?.Invoke();
                m_closeAction = null;
            }
        }

        // handlers
        private void OnMovePerformed(InputAction.CallbackContext a_ctx)
        {
            Vector2 v = a_ctx.ReadValue<Vector2>();
            m_moveInput = new Vector3(v.x, 0f, v.y);
            onMove?.Invoke(m_moveInput);
        }

        private void OnMoveCanceled(InputAction.CallbackContext a_ctx)
        {
            m_moveInput = Vector3.zero;
            onMove?.Invoke(m_moveInput);
        }

        private void OnSprintPerformed(InputAction.CallbackContext a_ctx)
        {
            onSprint?.Invoke(a_ctx.ReadValueAsButton());
            onMove?.Invoke(m_moveInput);
        }

        private void OnSprintCanceled(InputAction.CallbackContext a_ctx)
        {
            onSprint?.Invoke(false);
            onMove?.Invoke(m_moveInput);
        }

        private void OnInteractPerformed(InputAction.CallbackContext a_ctx)
        {
            if (m_player == null) return;
            var ic = m_player.Player_InteractionComponent;
            if (ic != null && ic.IsTriggered && ic.CurrentTarget != null)
                onInteract?.Invoke(ic.CurrentTarget);
        }

        // nettoyage public
        public void DoOnDestroy()
        {
            onMove = null;
            onSprint = null;
            onInteract = null;
            onOpenMenu = null;
            onCloseMenu = null;
        }
    }
}
