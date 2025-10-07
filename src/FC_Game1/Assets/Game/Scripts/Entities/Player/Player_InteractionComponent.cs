using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider))]
    public class Player_InteractionComponent : MonoBehaviour
    {
        public delegate void OnTriggerDelegate(Entity a_entity, IInteractable a_interactor, Transform a_target);
        #region properties
        private Player m_player;
        private Collider m_trigger;
        private IInteractable m_currentTarget;
        public IInteractable CurrentTarget => m_currentTarget;
        private bool m_isTriggered;
        public bool IsTriggered => m_isTriggered;
        public OnTriggerDelegate onTriggerDelegate;
        public OnTriggerDelegate onUnTriggerDelegate;
        #endregion

        public void Setup(Player a_player)
        {
            m_player = a_player;
            m_trigger = GetComponent<Collider>();
            m_trigger.isTrigger = true;
        }
        public void Init()
        {
            m_isTriggered = false;
        }
        public virtual void DoOnDestroy()
        {
            onTriggerDelegate = null;
            onUnTriggerDelegate = null;
        }

        void OnTriggerEnter(Collider a_other)
        {
            var interactable = a_other.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract(m_player))
            {
                m_currentTarget = interactable;
                onTriggerDelegate?.Invoke(m_player, interactable, a_other.transform);
                m_isTriggered = true;
            }
        }

        void OnTriggerExit(Collider a_other)
        {
            if (a_other.GetComponent<IInteractable>() == m_currentTarget)
            {
                onUnTriggerDelegate?.Invoke(m_player, m_currentTarget, a_other.transform);
                m_currentTarget = null;
                m_isTriggered = false;
            }
        }

        public void DoUpdate()
        {

        }

        public void ClearCurrentTarget()
        {
            if (m_currentTarget != null)
            {
                m_currentTarget = null;
                m_isTriggered = false;
            }
        }
    }
}
