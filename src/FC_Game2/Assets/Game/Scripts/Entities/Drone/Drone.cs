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
        public Drone_BatteryComponent BatteryComponent => m_batteryComponent;
        [SerializeField] private Drone_BatteryComponent m_batteryComponent;
        public Drone_VisualComponent VisualComponent => m_visualComponent;
        [SerializeField] private Drone_VisualComponent m_visualComponent;
        public Transform PlayerT => m_playerTransform;
        [SerializeField] private Transform m_playerTransform;
        public Transform Visual => m_visual;
        [SerializeField] private Transform m_visual;
        #endregion

        public override void Setup(GameContextScriptable a_context)
        {
            base.Setup(a_context);
            m_movementComponent.Setup(this);
            m_weaponComponent.Setup(this);
            m_sentinelComponent.Setup(this);
            m_batteryComponent.Setup(Vector3.zero); // TODO
            if (m_visualComponent != null && m_visual != null)
                m_visualComponent.Setup(this, m_visual);
        }
        public override void Init(GameState a_state)
        {
            base.Init(a_state);
            m_movementComponent.Init(m_playerTransform);
            m_weaponComponent.Init();
            m_sentinelComponent.Init();
            m_batteryComponent.Init();
            if (m_visualComponent != null)
                m_visualComponent.Init();
            m_movementComponent.Launch();
        }

        void Update()
        {
            m_movementComponent.DoUpdate();
            m_weaponComponent.DoUpdate();
            m_batteryComponent.DoUpdate(Time.deltaTime);
            if (m_visualComponent != null)
                m_visualComponent.DoUpdate();
        }
    }
}
