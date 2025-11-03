using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider))]
    public class Player_InteractionComponent : MonoBehaviour
    {
        public delegate void OnTriggerDelegate(Entity a_entity, IInteractable a_interactor, Transform a_target);

        private Player m_player;
        private Collider m_trigger;

        private readonly List<IInteractable> m_nearby = new();
        private IInteractable m_currentTarget;

        public IInteractable CurrentTarget => m_currentTarget;
        public bool IsTriggered => m_currentTarget != null;

        public OnTriggerDelegate onTriggerDelegate;
        public OnTriggerDelegate onUnTriggerDelegate;

        public void Setup(Player a_player)
        {
            m_player = a_player;
            m_trigger = GetComponent<Collider>();
            m_trigger.isTrigger = true;
        }

        public void Init()
        {
            m_nearby.Clear();
            m_currentTarget = null;
        }

        public virtual void DoOnDestroy()
        {
            onTriggerDelegate = null;
            onUnTriggerDelegate = null;
        }

        private void OnTriggerEnter(Collider a_other)
        {
            var interactable = a_other.GetComponent<IInteractable>();
            if (interactable == null || !interactable.CanInteract(m_player))
                return;

            if (!m_nearby.Contains(interactable))
                m_nearby.Add(interactable);

            UpdateCurrentTarget();
        }

        private void OnTriggerExit(Collider a_other)
        {
            var interactable = a_other.GetComponent<IInteractable>();
            if (interactable == null)
                return;

            if (m_nearby.Contains(interactable))
                m_nearby.Remove(interactable);

            if (interactable == m_currentTarget)
            {
                onUnTriggerDelegate?.Invoke(m_player, interactable, a_other.transform);
                UpdateCurrentTarget();
            }
        }

        public void DoUpdate()
        {
            // Vérifie régulièrement si la cible actuelle est toujours valide
            // if (m_currentTarget != null && !m_currentTarget.CanInteract(m_player))
            //     UpdateCurrentTarget();
            if (m_nearby.Count >= 2)
            {
                UpdateCurrentTarget();
            }
        }

        public void ClearCurrentTarget()
        {
            m_currentTarget = null;
        }
        public void RefreshInteractions()
        {
            // if (m_currentTarget != null && m_currentTarget.CanInteract(m_player))
            UpdateCurrentTarget();
        }

        private void UpdateCurrentTarget()
        {
            IInteractable best = null;
            float bestScore = float.MaxValue;

            foreach (var i in m_nearby)
            {
                if (i == null || !i.CanInteract(m_player))
                    continue;

                float score = Vector3.Distance(transform.position, ((MonoBehaviour)i).transform.position);
                if (score < bestScore)
                {
                    best = i;
                    bestScore = score;
                }
            }

            // Si la même target reste, on relance l’événement pour rafraîchir le prompt
            if (best == m_currentTarget)
            {
                if (m_currentTarget != null)
                    onTriggerDelegate?.Invoke(m_player, m_currentTarget, ((MonoBehaviour)m_currentTarget).transform);
                return;
            }

            // Si la cible change, on gère la transition complète
            if (m_currentTarget != null)
                onUnTriggerDelegate?.Invoke(m_player, m_currentTarget, ((MonoBehaviour)m_currentTarget).transform);

            m_currentTarget = best;

            if (m_currentTarget != null)
                onTriggerDelegate?.Invoke(m_player, m_currentTarget, ((MonoBehaviour)m_currentTarget).transform);
        }
    }
}