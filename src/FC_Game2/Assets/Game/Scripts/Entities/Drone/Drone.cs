using UnityEngine;

namespace Game
{
    public class Drone : Entity
    {
        #region properties
        public Drone_MovementComponent MovementComponent => m_movementComponent;
        [SerializeField] private Drone_MovementComponent m_movementComponent;
        public Drone_WeaponComponent WeaponComponent => m_weaponComponent;
        [SerializeField] private Drone_WeaponComponent m_weaponComponent;
        public Drone_SentinelComponent SentinelComponent => m_sentinelComponent;
        [SerializeField] private Drone_SentinelComponent m_sentinelComponent;
        public Transform PlayerT => m_playerTransform;
        [SerializeField] private Transform m_playerTransform;
        #endregion

        public override void Setup(GameContextScriptable a_context)
        {
            base.Setup(a_context);
            m_movementComponent.Setup();
            m_weaponComponent.Setup(this);
            m_sentinelComponent.Setup(this);
        }
        public override void Init(GameState a_state)
        {
            base.Init(a_state);
            m_movementComponent.Init(m_playerTransform);
            m_weaponComponent.Init();
            m_sentinelComponent.Init();
        }

        void Update()
        {
            m_movementComponent.DoUpdate();
            m_weaponComponent.DoUpdate();
        }
    }
}
