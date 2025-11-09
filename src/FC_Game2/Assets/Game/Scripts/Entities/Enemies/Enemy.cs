using UnityEngine;

namespace Game
{
    public class Enemy : MonoBehaviour
    {
        #region properties
        public Enemy_MovementComponent MovementComponent => m_movementComponent;
        [SerializeField] private Enemy_MovementComponent m_movementComponent;
        public Enemy_HealthComponent HealthComponent => m_healthComponent;
        [SerializeField] private Enemy_HealthComponent m_healthComponent;
        public Enemy_Controller Controller => m_controller;
        [SerializeField] private Enemy_Controller m_controller;
        public Rigidbody Rigidbody => m_rigidbody;
        [SerializeField] private Rigidbody m_rigidbody;

        public EnemyManagerGrid Manager => m_manager;
        private EnemyManagerGrid m_manager;
        public int Id => m_id;
        private int m_id;
        public EnemyManagerGrid.EnemyData Data => m_data;
        private EnemyManagerGrid.EnemyData m_data;
        public Transform PlayerT => m_playerT;
        private Transform m_playerT;
        #endregion

        public void Setup(EnemyManagerGrid a_manager, int a_id, EnemyManagerGrid.EnemyData a_data, Transform a_playerT)
        {
            // GetComponent<Renderer>().material.DisableKeyword("_USEBUFFER_ON"); 
            m_manager = a_manager;
            m_id = a_id;
            m_data = a_data;
            m_playerT = a_playerT;
            m_movementComponent.Setup(this);
            m_healthComponent.Setup(this, new FCTools.FloatableParam(m_data.health));
            m_controller.Setup(this);
        }
        public void Init()
        {
            m_movementComponent.Init(m_playerT, m_data.velocity);
            m_healthComponent.Init(new FCTools.FloatableParam(m_data.health));
            m_controller.Init();
            gameObject.SetActive(true);
        }

        void Update()
        {
            m_movementComponent.DoUpdate();
        }
    }
}
