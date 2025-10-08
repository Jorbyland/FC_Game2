using UnityEngine;

namespace Game
{
    public class Building_TriggerComponent : MonoBehaviour
    {
        #region properties
        private Building m_building;
        private bool m_playerInside;
        public bool PlayerIsInside => m_playerInside;
        private System.Action<bool> m_onDetectPlayer;
        #endregion

        public void Setup(Building a_building)
        {
            m_building = a_building;
        }
        public void Init(System.Action<bool> a_onDetectPlayer)
        {
            m_onDetectPlayer = a_onDetectPlayer;
        }

        private void OnTriggerEnter(Collider a_other)
        {
            // if (!a_other.TryGetComponent<Player>(out _))
            //     return;

            if (m_playerInside) return;

            m_playerInside = true;
            m_onDetectPlayer?.Invoke(true);
        }

        private void OnTriggerExit(Collider a_other)
        {
            // if (!a_other.TryGetComponent<Player>(out _))
            //     return;

            if (!m_playerInside) return;

            m_playerInside = false;
            m_onDetectPlayer?.Invoke(false);
        }
    }
}
