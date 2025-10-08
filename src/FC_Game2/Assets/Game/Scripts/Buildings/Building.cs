using UnityEngine;

namespace Game
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Transform m_exteriorBase;
        [SerializeField] private Transform m_exterior;
        [SerializeField] private Transform m_interior;
        [SerializeField] private Transform m_colliders;

        [SerializeField] private Building_TriggerComponent m_triggerComponent;
        public Building_TriggerComponent TriggerCOmponent => m_triggerComponent;
        private bool m_isHidden;

        private void Start()
        {
            SwitchToExterior();
            m_triggerComponent.Setup(this);
            m_triggerComponent.Init(OnDetectPlayer);
        }

        public void Hide(bool a_hide)
        {
            if (m_isHidden == a_hide || m_triggerComponent.PlayerIsInside) return;
            m_isHidden = a_hide;
            HideExterior(m_isHidden);
        }

        private void OnDetectPlayer(bool a_inside)
        {
            if (a_inside)
            {
                SwitchToInterior();
                CameraManager.Instance.SwitchToInterior();
            }
            else
            {
                SwitchToExterior();
                CameraManager.Instance.SwitchToExterior();
            }
        }
        private void SwitchToInterior()
        {
            if (m_interior != null)
                m_interior.gameObject.SetActive(true);
            if (m_exterior != null)
                m_exterior.gameObject.SetActive(false);
            if (m_exteriorBase != null)
                m_exteriorBase.gameObject.SetActive(false);
            if (m_colliders != null)
                m_colliders.gameObject.SetActive(false);
        }
        private void SwitchToExterior()
        {
            if (m_interior != null)
                m_interior.gameObject.SetActive(false);
            if (m_exterior != null)
                m_exterior.gameObject.SetActive(!m_isHidden);
            if (m_exteriorBase != null)
                m_exteriorBase.gameObject.SetActive(m_isHidden);
            if (m_colliders != null)
                m_colliders.gameObject.SetActive(true);
        }

        private void HideExterior(bool a_value)
        {
            if (m_exterior != null)
                m_exterior.gameObject.SetActive(!a_value);
            if (m_exteriorBase != null)
                m_exteriorBase.gameObject.SetActive(a_value);
        }
    }
}