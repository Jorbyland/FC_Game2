using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraObstacleDetector : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform m_target;

        [Header("Detection Settings")]
        [SerializeField] private LayerMask m_outdoorLayer;
        [SerializeField] private LayerMask m_indoorLayer;
        [SerializeField] private float m_checkRadiusOutdoor = 0.2f;
        [SerializeField] private float m_checkRadiusIndoor = 2.5f;
        [SerializeField] private float m_maxDistance = 50f;

        [Header("Indoor Volume Settings")]
        [Tooltip("Rayon de recherche autour du joueur pour masquer les murs proches en intérieur.")]
        [SerializeField] private float m_innerDetectionRadius = 4f;
        [SerializeField] private float m_innerVerticalTolerance = 3f;

        private Camera m_camera;
        private readonly HashSet<Building> m_currentBuildings = new();
        private readonly RaycastHit[] m_hitsBuffer = new RaycastHit[32];

        private Building m_currentBuilding;

        private void Awake()
        {
            m_camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (m_target == null)
                return;

            if (m_currentBuilding == null)
                DetectOutdoorObstacles();
        }

        private void DetectOutdoorObstacles()
        {
            Vector3 start = transform.position;
            Vector3 end = m_target.position;
            Vector3 dir = (end - start).normalized;
            float dist = Vector3.Distance(start, end);

            int hitCount = Physics.SphereCastNonAlloc(
                start, m_checkRadiusOutdoor, dir,
                m_hitsBuffer, dist, m_outdoorLayer,
                QueryTriggerInteraction.Ignore
            );

            HashSet<Building> newHits = new();

            for (int i = 0; i < hitCount; i++)
            {
                var b = m_hitsBuffer[i].collider.GetComponentInParent<Building>();
                if (b == null) continue;
                newHits.Add(b);
                if (!m_currentBuildings.Contains(b))
                {
                    b.Hide(true);
                    m_currentBuildings.Add(b);
                }
            }

            foreach (var b in m_currentBuildings)
                if (!newHits.Contains(b))
                    b.Hide(false);

            m_currentBuildings.IntersectWith(newHits);
        }

        public void SetTarget(Transform target) => m_target = target;

        public void SetCurrentBuilding(Building building)
        {
            m_currentBuilding = building;

            foreach (var b in m_currentBuildings)
                b.Hide(false);
            m_currentBuildings.Clear();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_target == null || m_currentBuilding == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(m_target.position, m_innerDetectionRadius);
        }
#endif
    }
}