using UnityEngine;

namespace Game
{
    public class Drone_WeaponComponent : MonoBehaviour
    {
        #region inspector
        [SerializeField] private float m_fireRate = 0.3f;
        [SerializeField] private float m_fireDamage = 0.3f;
        [SerializeField] private float m_fireAngle = 5f;
        [SerializeField] private float m_fireRadius = 3f;

        [SerializeField] private Transform m_playerTarget;
        [SerializeField] private LayerMask m_enemyLayerMask;
        [SerializeField] private GameObject m_shootImpactFX;
        [SerializeField] private GameObject m_creepExplosionFX;
        #endregion
        #region properties
        private Drone m_drone;
        private float m_fireRateTimer = 0;
        private float m_angleArroundTarget = 0;
        #endregion

        public void Setup(Drone a_drone)
        {
            m_drone = a_drone;
        }
        public void Init()
        {
            m_fireRateTimer = m_fireRate;
            m_angleArroundTarget = 0;
        }

        public void DoUpdate()
        {
            if (m_fireRateTimer <= 0)
            {
                // Fire();
                FireAutoTargeting();
                m_fireRateTimer = m_fireRate;
            }
            else
            {
                m_fireRateTimer -= Time.deltaTime;
            }
        }

        private void Fire()
        {
            Vector3 targetPos = GetPositionArroundPlayer();
            if (Physics.SphereCast(transform.position, 0.5f, (targetPos - transform.position).normalized, out RaycastHit hit,
                         Vector3.Distance(targetPos, transform.position), m_enemyLayerMask, QueryTriggerInteraction.Ignore))
            {
                EnemyHybridBridge enemyHybridBridge = hit.collider.gameObject.GetComponent<EnemyHybridBridge>();
                enemyHybridBridge.Kill();
                // Destroy(enemyHybridBridge.gameObject);
            }
            else
            {
                // TODO FX shoot at targetPos
                Instantiate(m_shootImpactFX, targetPos, Quaternion.identity);
            }
        }
        private void FireAutoTargeting()
        {
            Enemy enemy = m_drone.SentinelComponent.GetNearestEnemyInRange();
            if (enemy != null)
            {
                Instantiate(m_creepExplosionFX, enemy.gameObject.transform.position, Quaternion.identity);
                enemy.HealthComponent.Damage(m_fireDamage, Vector3.zero);
            }
        }

        private Vector3 GetPositionArroundPlayer()
        {
            float angleRadians = m_angleArroundTarget * Mathf.Deg2Rad;
            m_angleArroundTarget += m_fireAngle;
            Vector3 offset = new Vector3(Mathf.Cos(angleRadians), 0f, Mathf.Sin(angleRadians)) * m_fireRadius;
            offset = m_playerTarget.rotation * offset;
            return m_playerTarget.position + offset;
        }
    }
}
