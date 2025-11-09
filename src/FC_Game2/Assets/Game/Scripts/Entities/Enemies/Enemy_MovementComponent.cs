using UnityEngine;

namespace Game
{
    public class Enemy_MovementComponent : MonoBehaviour
    {
        #region properties
        private Enemy m_enemy;
        private Transform m_playerT;
        private Vector3 m_velocity;
        #endregion

        public void Setup(Enemy a_enemy)
        {
            m_enemy = a_enemy;
        }
        public void Init(Transform a_playerT, Vector3 a_velocity)
        {
            m_playerT = a_playerT;
            m_velocity = a_velocity;
        }

        public void DoUpdate()
        {
            Vector3 dir = (m_playerT.position - transform.position).normalized;
            m_enemy.Rigidbody.linearVelocity = dir * m_velocity.magnitude;
            // transform.Translate(dir * m_velocity.magnitude * Time.deltaTime, Space.World);
            // transform.LookAt(m_playerT);
        }
    }
}
