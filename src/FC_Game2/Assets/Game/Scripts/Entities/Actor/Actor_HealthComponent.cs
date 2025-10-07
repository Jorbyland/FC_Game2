using UnityEngine;
using FCTools;

namespace Game
{
    public class Actor_HealthComponent : MonoBehaviour
    {
        public delegate void OnReceivedDamage(Entity a_entity, float a_damage, Vector3 a_knockbackVector);
        public delegate void OnHealthChanged(Entity a_entity, float a_currentHealth, bool a_isHeal);
        public delegate void OnDeath(Entity a_entity);

        #region Properties
        protected Entity m_entity;
        public Entity Entity => m_entity;

        public float CurrentHealth => m_currentHealth;
        protected float m_currentHealth;

        public bool IsAlive => m_currentHealth > 0f;

        public FloatableParam MaxHealth => m_maxHealth;
        protected FloatableParam m_maxHealth;

        public float LastDamageReceivedTime => m_lastDamageReceivedTime;
        protected float m_lastDamageReceivedTime;

        public OnReceivedDamage onReceivedDamage;
        public OnHealthChanged onHealthChanged;
        public OnDeath onDeath;
        #endregion


        #region Lifecycle
        public virtual void Setup(Entity a_entity, FloatableParam a_maxHealth)
        {
            m_maxHealth = a_maxHealth;
            m_entity = a_entity;
        }
        public virtual void Init(FloatableParam a_currentHealth)
        {
            m_currentHealth = a_currentHealth.Value;
        }

        protected virtual void OnDestroy()
        {
            onDeath = null;
            onHealthChanged = null;
            onReceivedDamage = null;
        }
        #endregion

        public virtual void Damage(float a_damageToInflict, Vector3 a_knockbackVector)
        {
            if (!IsAlive)
                return;

            if (a_damageToInflict < 0f)
            {
                Debug.LogError($"Trying to deal negative damages to {m_entity.gameObject.name} -> {a_damageToInflict}", m_entity.gameObject);
                return;
            }
            m_currentHealth = Mathf.MoveTowards(m_currentHealth, 0f, a_damageToInflict);
            onReceivedDamage?.Invoke(m_entity, a_damageToInflict, a_knockbackVector);
            onHealthChanged?.Invoke(m_entity, m_currentHealth, false);
            m_lastDamageReceivedTime = Time.time;

            if (!IsAlive)
                InternalSetDead();
        }

        public virtual void RestoreHealth(float a_healthToRestore)
        {
            if (!IsAlive)
                return;

            if (a_healthToRestore < 0f)
            {
                Debug.LogError($"Trying to heal negative damages to {m_entity.gameObject.name} -> {a_healthToRestore}", m_entity.gameObject);
                return;
            }

            m_currentHealth = Mathf.MoveTowards(m_currentHealth, m_maxHealth.Value, a_healthToRestore);
            onHealthChanged?.Invoke(m_entity, m_currentHealth, true);
        }

        public virtual void IncraseMaxHealth(FloatableParam a_maxHealth)
        {
            if (!IsAlive)
                return;
            float lastMaxHealth = m_maxHealth.Value;
            m_currentHealth = m_currentHealth * m_maxHealth.Value / lastMaxHealth;
            m_maxHealth = a_maxHealth;
            onHealthChanged?.Invoke(m_entity, m_currentHealth, true);
        }

        public float GetHealthRatio(float a_health)
        {
            return Mathf.InverseLerp(0, m_maxHealth.Value, a_health);
        }

        protected virtual void InternalSetDead()
        {
            m_currentHealth = 0f;
            onDeath?.Invoke(m_entity);
        }
    }
}
