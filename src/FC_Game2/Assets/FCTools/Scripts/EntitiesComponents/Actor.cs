using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FCTools.EntitiesComponents
{
    [RequireComponent(typeof(Rigidbody))]
    public class Actor : Entity
    {
        #region inspector
        public Actor_TriggerDetetectionComponent TriggerDetection => m_triggerDetection;
        [SerializeField] private Actor_TriggerDetetectionComponent m_triggerDetection;
        public Actor_HealthComponent HealthComponent => m_healthComponent;
        [SerializeField] private Actor_HealthComponent m_healthComponent;

        #endregion

        #region properties

        public Rigidbody Rigidbody => m_rigidbody;
        private Rigidbody m_rigidbody;

        #endregion
        public override void Setup()
        {
            base.Setup();
            m_rigidbody = GetComponent<Rigidbody>();
            m_triggerDetection.Setup();
            m_healthComponent.Setup(this);
        }
    }
}
