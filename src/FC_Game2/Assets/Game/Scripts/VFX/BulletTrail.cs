using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class BulletTrail : MonoBehaviour
    {
        [SerializeField] private float m_speed = 40;
        #region properties
        private Vector3 m_startPosition;
        private Vector3 m_targetPosition;
        private float m_progress;
        #endregion

        void Update()
        {
            m_progress += Time.deltaTime * m_speed;
            transform.position = Vector3.Lerp(m_startPosition, m_targetPosition, m_progress);
            if (m_progress >= 1)
            {
                gameObject.SetActive(false);
            }
        }

        public void Init(Vector3 a_startPosition, Vector3 a_targetPosition)
        {
            m_progress = 0;
            transform.position = m_startPosition = a_startPosition;
            m_startPosition = a_startPosition;
            m_targetPosition = a_targetPosition;
            gameObject.SetActive(true);
        }
    }
}
