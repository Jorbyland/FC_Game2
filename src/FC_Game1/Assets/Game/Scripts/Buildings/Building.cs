using UnityEngine;

namespace Game
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Transform m_exteriors;
        [SerializeField] private Transform m_interiors;

        private bool m_playerInside;

        private void Start()
        {
            SetInterior(false);
        }

        private void OnTriggerEnter(Collider a_other)
        {
            // if (!a_other.TryGetComponent<Player>(out _))
            //     return;

            if (m_playerInside) return;

            m_playerInside = true;
            SetInterior(true);
            CameraManager.Instance.SwitchToInterior();
        }

        private void OnTriggerExit(Collider a_other)
        {
            // if (!a_other.TryGetComponent<Player>(out _))
            //     return;

            if (!m_playerInside) return;

            m_playerInside = false;
            SetInterior(false);
            CameraManager.Instance.SwitchToExterior();
        }

        private void SetInterior(bool a_active)
        {
            if (m_exteriors != null)
                m_exteriors.gameObject.SetActive(!a_active);
            if (m_interiors != null)
                m_interiors.gameObject.SetActive(a_active);
        }
    }
}