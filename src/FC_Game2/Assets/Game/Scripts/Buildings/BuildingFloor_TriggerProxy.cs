using UnityEngine;

namespace Game
{
    public class BuildingFloor_TriggerProxy : MonoBehaviour
    {
        private BuildingFloor_TriggerComponent m_parent;
        private Collider m_collider;

        public void Setup(BuildingFloor_TriggerComponent parent)
        {
            m_parent = parent;
            m_collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            m_parent?.NotifyTriggerEnter(m_collider);
        }

        private void OnTriggerExit(Collider other)
        {
            m_parent?.NotifyTriggerExit(m_collider);
        }
    }
}