using UnityEngine;

namespace Game
{
    public class Drone_MovementComponent : MonoBehaviour
    {
        private enum DroneMovementState
        {
            IdleOnGround,
            TakeOff,
            FollowingPlayer,
            ReturningToCharge,
            Landing
        }

        [Header("Movement Settings")]
        [SerializeField] private float m_hoverHeight = 8f;
        [SerializeField] private float m_transitionSpeed = 2.5f;
        [SerializeField] private float m_moveSpeed = 5f;
        [SerializeField] private float m_followRadius = 6f;
        [SerializeField] private float m_randomMoveInterval = 3f;
        [SerializeField] private float m_landingHeight = 0.5f;

        private Drone m_drone;
        private DroneMovementState m_state;
        private Transform m_playerT;
        private Transform m_droneT;
        private Drone_BatteryComponent m_battery;

        private Vector3 m_currentTarget;
        private float m_nextMoveTimer;

        private Vector3 m_followBasePosition; // cible de base pour vol
        private Vector3 m_chargePoint;
        private bool m_descending;

        public void Setup(Drone a_drone)
        {
            m_drone = a_drone;
            m_droneT = a_drone.transform;
            m_battery = a_drone.BatteryComponent;
            m_state = DroneMovementState.IdleOnGround;
        }

        public void Init(Transform playerT)
        {
            m_playerT = playerT;
        }

        public void DoUpdate()
        {
            if (m_droneT == null) return;

            switch (m_state)
            {
                case DroneMovementState.IdleOnGround:
                    UpdateIdle();
                    break;
                case DroneMovementState.TakeOff:
                    UpdateTakeOff();
                    break;
                case DroneMovementState.FollowingPlayer:
                    UpdateFollowing();
                    break;
                case DroneMovementState.ReturningToCharge:
                    UpdateReturning();
                    break;
                case DroneMovementState.Landing:
                    UpdateLanding();
                    break;
            }

            if (m_battery != null && m_battery.IsEmpty && m_state != DroneMovementState.ReturningToCharge)
                SetState(DroneMovementState.ReturningToCharge);
        }

        private void SetState(DroneMovementState newState)
        {
            m_state = newState;
            if (newState == DroneMovementState.FollowingPlayer)
                m_nextMoveTimer = 0f;
        }

        private void UpdateIdle()
        {
            if (m_battery != null && !m_battery.IsRecharging)
                m_battery.StartRecharge(m_droneT.position);
        }

        private void UpdateTakeOff()
        {
            Vector3 target = new Vector3(m_playerT.position.x, m_playerT.position.y + m_hoverHeight, m_playerT.position.z);
            m_droneT.position = Vector3.Lerp(m_droneT.position, target, m_transitionSpeed * Time.deltaTime);
            if (Vector3.Distance(m_droneT.position, target) < 0.1f)
            {
                m_followBasePosition = m_droneT.position;
                SetState(DroneMovementState.FollowingPlayer);
            }
        }

        private void UpdateFollowing()
        {
            m_nextMoveTimer -= Time.deltaTime;
            if (m_nextMoveTimer <= 0f)
            {
                Vector2 rand = Random.insideUnitCircle * m_followRadius;
                m_currentTarget = m_playerT.position + new Vector3(rand.x, 0, rand.y);
                m_currentTarget.y = m_playerT.position.y + m_hoverHeight;
                m_nextMoveTimer = m_randomMoveInterval;
            }

            m_droneT.position = Vector3.Lerp(m_droneT.position, m_currentTarget, m_moveSpeed * Time.deltaTime);
        }

        private void UpdateReturning()
        {
            if (m_battery == null) return;

            if (!m_descending)
            {
                Vector3 chargePos = m_battery.LastChargePoint;
                chargePos.y = m_playerT.position.y + m_hoverHeight;
                m_droneT.position = Vector3.MoveTowards(m_droneT.position, chargePos, m_moveSpeed * Time.deltaTime);

                if (Vector3.Distance(m_droneT.position, chargePos) < 0.2f)
                {
                    m_chargePoint = m_battery.LastChargePoint;
                    m_descending = true;
                    SetState(DroneMovementState.Landing);
                }
            }
        }

        private void UpdateLanding()
        {
            Vector3 target = m_chargePoint + Vector3.up * m_landingHeight;
            m_droneT.position = Vector3.MoveTowards(m_droneT.position, target, m_transitionSpeed * Time.deltaTime);
            if (Vector3.Distance(m_droneT.position, target) < 0.05f)
            {
                m_battery.StartRecharge(m_chargePoint);
                m_descending = false;
                SetState(DroneMovementState.IdleOnGround);
            }
        }

        public void Launch()
        {
            if (m_state == DroneMovementState.IdleOnGround)
                SetState(DroneMovementState.TakeOff);
        }
    }
}