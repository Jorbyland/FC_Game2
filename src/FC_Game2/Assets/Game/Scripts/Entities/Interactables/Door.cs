using UnityEngine;

namespace Game
{
    public class Door : MonoBehaviour, IInteractable
    {
        #region inspector
        [SerializeField] protected Transform m_pivot;
        [SerializeField] protected Collider m_doorCollider;
        private bool m_isOpen = false;
        private bool m_canInteract;

        public bool CanInteract(Entity a_interactor)
        {
            return true;
        }

        public string GetInteractionPrompt()
        {
            return m_isOpen ? "Close" : "Open";
        }

        public void OnInteractionCompleted()
        {
            m_canInteract = true;
        }

        public void Interact(Entity a_interactor)
        {
            m_canInteract = false;
            if (m_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
        #endregion

        private void Open()
        {
            if (m_isOpen) return;
            m_isOpen = true;
            m_pivot.Rotate(Vector3.up, 90);
            m_doorCollider.enabled = false;
            OnInteractionCompleted();
        }
        private void Close()
        {
            if (!m_isOpen) return;
            m_isOpen = false;
            m_pivot.Rotate(Vector3.up, -90);
            m_doorCollider.enabled = true;
            OnInteractionCompleted();
        }
    }
}
