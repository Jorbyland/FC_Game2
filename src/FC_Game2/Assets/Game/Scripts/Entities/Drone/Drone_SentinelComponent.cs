using System.Collections.Generic;
using UnityEngine;
using FCTools;

namespace Game
{
    public class Drone_SentinelComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask m_enemyLayerMask;
        [SerializeField] private float m_detectionRange;
        #region properties
        private Drone m_drone;
        #endregion

        public void Setup(Drone a_drone)
        {
            m_drone = a_drone;
        }
        public void Init()
        {

        }

        private List<Enemy> DetectEnemiesInRange()
        {
            List<Enemy> result = new List<Enemy>();
            Collider[] colliders = Physics.OverlapSphere(m_drone.PlayerT.position, m_detectionRange,
                     m_enemyLayerMask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    result.Add(colliders[i].gameObject.GetComponent<Enemy>());
                }
            }
            return result;
        }

        public Enemy GetNearestEnemyInRange()
        {
            List<Enemy> enemiesInRange = DetectEnemiesInRange();
            if (enemiesInRange.Count > 0)
            {
                return m_drone.PlayerT.position.GetClosestElement(enemiesInRange.ToArray(), 0);
            }
            else
            {
                return null;
            }
        }
    }
}
