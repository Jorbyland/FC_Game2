using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraObstacleDetector : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private Transform m_target;                   // Joueur
        [SerializeField] private LayerMask m_obstacleLayer;            // Layer "BuildingCollider"
        [SerializeField] private float m_checkRadius = 0.2f;
        [SerializeField] private float m_maxDistance = 50f;

        #region properties
        private Camera m_camera;
        private readonly HashSet<Building> m_currentObstacles = new();
        private readonly RaycastHit[] m_hitsBuffer = new RaycastHit[16];
        #endregion

        private void Awake()
        {
            m_camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (m_target == null) return;
            DetectObstacles();
        }

        private void DetectObstacles()
        {
            Vector3 start = transform.position;
            Vector3 end = m_target.position;
            Vector3 dir = (end - start).normalized;
            float dist = Vector3.Distance(start, end);

            int hitCount = Physics.SphereCastNonAlloc(
                start, m_checkRadius, dir,
                m_hitsBuffer, dist, m_obstacleLayer,
                QueryTriggerInteraction.Ignore
            );

            HashSet<Building> newHits = new();

            for (int i = 0; i < hitCount; i++)
            {
                Building building = m_hitsBuffer[i].collider.GetComponentInParent<Building>();
                if (building == null) continue;
                newHits.Add(building);
                if (!m_currentObstacles.Contains(building))
                {
                    building.Hide(true);
                    m_currentObstacles.Add(building);
                }
            }

            // Nettoyage des obstacles non détectés
            foreach (var b in m_currentObstacles)
            {
                if (!newHits.Contains(b))
                    b.Hide(false);
            }
            m_currentObstacles.IntersectWith(newHits);
        }

        public void SetTarget(Transform target)
        {
            m_target = target;
        }
    }
}
