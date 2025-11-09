using UnityEngine;

namespace Game
{
    public class Drone_BatteryComponent : MonoBehaviour
    {
        [SerializeField] private float m_maxBattery = 100f;
        [SerializeField] private float m_currentBattery = 100f;
        [SerializeField] private float m_drainRate = 2f;
        [SerializeField] private float m_rechargeRate = 10f;

        private bool m_isRecharging;
        private Vector3 m_lastChargePoint;

        public bool IsEmpty => m_currentBattery <= 0f;
        public bool IsRecharging => m_isRecharging;
        public float BatteryRatio => m_currentBattery / m_maxBattery;
        public Vector3 LastChargePoint => m_lastChargePoint;

        public void Setup(Vector3 initialChargePoint)
        {
            m_lastChargePoint = initialChargePoint;
            m_currentBattery = m_maxBattery;
        }

        public void Init()
        {

        }

        public void DoUpdate(float deltaTime)
        {
            if (m_isRecharging)
                Recharge(deltaTime);
            else
                Drain(deltaTime);
        }

        private void Drain(float deltaTime)
        {
            m_currentBattery -= m_drainRate * deltaTime;
            if (m_currentBattery < 0f)
                m_currentBattery = 0f;
        }

        private void Recharge(float deltaTime)
        {
            m_currentBattery += m_rechargeRate * deltaTime;
            if (m_currentBattery >= m_maxBattery)
            {
                m_currentBattery = m_maxBattery;
                m_isRecharging = false;
            }
        }

        public void StartRecharge(Vector3 rechargePoint)
        {
            m_lastChargePoint = rechargePoint;
            m_isRecharging = true;
        }

        public void StopRecharge() => m_isRecharging = false;
    }
}