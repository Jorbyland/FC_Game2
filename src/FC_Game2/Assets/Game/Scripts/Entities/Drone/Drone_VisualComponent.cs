using UnityEngine;

namespace Game
{
    public class Drone_VisualComponent : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] private float m_maxTiltAngle = 15f;
        [SerializeField] private float m_tiltSmoothing = 5f;
        [SerializeField] private bool m_tiltOnX = true;
        [SerializeField] private bool m_tiltOnZ = true;

        private Drone m_drone;
        private Transform m_visualT;
        private Vector3 m_currentVelocity;
        private Vector3 m_smoothedVelocity;

        public void Setup(Drone a_drone, Transform a_visualT)
        {
            m_drone = a_drone;
            m_visualT = a_visualT;
        }

        public void Init()
        {
            m_currentVelocity = Vector3.zero;
            m_smoothedVelocity = Vector3.zero;
        }

        public void DoUpdate()
        {
            if (m_visualT == null || m_drone == null) return;

            // Récupérer la direction de mouvement depuis le composant de mouvement
            Vector3 movementDirection = m_drone.MovementComponent.GetMovementDirection();
            
            // Calculer la vélocité basée sur la direction
            m_currentVelocity = movementDirection * m_drone.MovementComponent.MoveSpeed;
            
            // Lisser la vélocité pour des transitions plus fluides
            m_smoothedVelocity = Vector3.Lerp(m_smoothedVelocity, m_currentVelocity, m_tiltSmoothing * Time.deltaTime);

            // Calculer l'inclinaison basée sur la vélocité horizontale
            Vector3 horizontalVelocity = new Vector3(m_smoothedVelocity.x, 0f, m_smoothedVelocity.z);
            float velocityMagnitude = horizontalVelocity.magnitude;
            
            // Normaliser pour obtenir la direction
            Vector3 tiltDirection = velocityMagnitude > 0.01f ? horizontalVelocity.normalized : Vector3.zero;
            
            // Calculer les angles d'inclinaison
            float tiltX = 0f;
            float tiltZ = 0f;
            
            if (m_tiltOnX && tiltDirection.magnitude > 0.01f)
            {
                // Inclinaison sur l'axe X (pitch) basée sur la direction Z
                tiltX = -tiltDirection.z * m_maxTiltAngle * Mathf.Clamp01(velocityMagnitude / m_drone.MovementComponent.MoveSpeed);
            }
            
            if (m_tiltOnZ && tiltDirection.magnitude > 0.01f)
            {
                // Inclinaison sur l'axe Z (roll) basée sur la direction X
                tiltZ = tiltDirection.x * m_maxTiltAngle * Mathf.Clamp01(velocityMagnitude / m_drone.MovementComponent.MoveSpeed);
            }

            // Appliquer la rotation d'inclinaison (préserver la rotation Y du parent ou utiliser la direction de mouvement)
            float currentY = m_visualT.eulerAngles.y;
            if (tiltDirection.magnitude > 0.01f)
            {
                // Orienter le drone dans la direction du mouvement
                currentY = Mathf.Atan2(tiltDirection.x, tiltDirection.z) * Mathf.Rad2Deg;
            }
            Quaternion targetRotation = Quaternion.Euler(tiltX, currentY, tiltZ);
            m_visualT.rotation = Quaternion.Slerp(m_visualT.rotation, targetRotation, m_tiltSmoothing * Time.deltaTime);
        }
    }
}

