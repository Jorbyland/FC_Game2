using UnityEngine;

namespace Game
{
    public class Drone_MovementComponent : MonoBehaviour
    {
        [SerializeField] private float m_flightAltitude = 5f;
        #region properties
        private Transform m_target;
        #endregion

        public void Setup()
        {

        }
        public void Init(Transform a_target)
        {
            m_target = a_target;
        }

        public void DoUpdate()
        {
            Vector3 targetPos = new Vector3(m_target.position.x, m_flightAltitude, m_target.position.z);
            Vector3 dronePos = new Vector3(transform.position.x, m_flightAltitude, transform.position.z);
            transform.position = Vector3.Lerp(dronePos, targetPos, 0.5f * Time.deltaTime);
        }
    }
}
