using FCTools;
using UnityEngine;

namespace Game
{
    public class Enemy_HealthComponent : MonoBehaviour
    {
        public delegate void OnReceivedDamage(float a_damage, Vector3 a_knockbackVector);
        public delegate void OnDeath(Enemy a_enemy);

        #region Properties
        private Enemy m_enemy;
        public float CurrentHealth => m_currentHealth;
        protected float m_currentHealth;

        public bool IsAlive => m_currentHealth > 0f;

        public FloatableParam MaxHealth => m_maxHealth;
        protected FloatableParam m_maxHealth;

        public float LastDamageReceivedTime => m_lastDamageReceivedTime;
        protected float m_lastDamageReceivedTime;

        public OnReceivedDamage onReceivedDamage;
        public OnDeath onDeath;
        #endregion


        #region Lifecycle
        public virtual void Setup(Enemy a_enemy, FloatableParam a_maxHealth)
        {
            m_maxHealth = a_maxHealth;
            m_enemy = a_enemy;
        }
        public virtual void Init(FloatableParam a_currentHealth)
        {
            m_currentHealth = a_currentHealth.Value;
        }

        protected virtual void OnDestroy()
        {
            onDeath = null;
            onReceivedDamage = null;
        }
        #endregion

        public virtual void Damage(float a_damageToInflict, Vector3 a_knockbackVector)
        {
            if (!IsAlive)
                return;

            if (a_damageToInflict < 0f)
            {
                return;
            }
            m_currentHealth = Mathf.MoveTowards(m_currentHealth, 0f, a_damageToInflict);
            onReceivedDamage?.Invoke(a_damageToInflict, a_knockbackVector);
            m_lastDamageReceivedTime = Time.time;

            if (!IsAlive)
                InternalSetDead();
        }

        public float GetHealthRatio(float a_health)
        {
            return Mathf.InverseLerp(0, m_maxHealth.Value, a_health);
        }

        protected virtual void InternalSetDead()
        {
            m_currentHealth = 0f;
            onDeath?.Invoke(m_enemy);
        }
    }
}
