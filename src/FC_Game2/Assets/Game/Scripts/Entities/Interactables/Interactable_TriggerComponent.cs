using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider))]
    public class Interactable_TriggerComponent : MonoBehaviour
    {
        public delegate void OnTriggerDelegate(Entity a_entity, Entity a_whoTriggerEntity);
        #region properties
        private Entity m_entity;
        private Collider m_trigger;
        private bool m_isTriggered;
        public OnTriggerDelegate onTriggerDelegate;
        public OnTriggerDelegate onUnTriggerDelegate;
        #endregion

        public virtual void Setup(Entity a_entity)
        {
            m_entity = a_entity;
            m_trigger = GetComponent<Collider>();
            m_trigger.isTrigger = true;
        }

        public virtual void Init()
        {
            m_isTriggered = false;
        }

        public virtual void DoOnDestroy()
        {
            onTriggerDelegate = null;
            onUnTriggerDelegate = null;
        }


        public void Trigger(Entity a_extEntity)
        {
            if (m_isTriggered) return;
            m_isTriggered = true;
            onTriggerDelegate?.Invoke(m_entity, a_extEntity);
        }

        public void UnTrigger(Entity a_extEntity)
        {
            if (!m_isTriggered) return;
            m_isTriggered = false;
            onUnTriggerDelegate?.Invoke(m_entity, a_extEntity);
        }
    }
}
