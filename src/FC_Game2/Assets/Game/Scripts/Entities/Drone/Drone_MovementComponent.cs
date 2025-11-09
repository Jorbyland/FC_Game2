using UnityEngine;
using FCTools.MovementBehavior;

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
        [SerializeField] private float m_landingHeight = 0.5f;
        [SerializeField] private int m_directionCount = 8;

        [Header("Movement Behaviors")]
        [SerializeField] private ChaseMovementBehavior m_chaseBehavior;
        [SerializeField] private RandomMovementBehavior m_randomBehavior;
        [SerializeField] private FocusMovementBehavior m_focusBehavior;
        [SerializeField] private RotateArroundMovementBehaviour m_rotateBehavior;

        private Drone m_drone;
        private DroneMovementState m_state;
        private Transform m_playerT;
        private Transform m_droneT;
        private Drone_BatteryComponent m_battery;

        private Vector3[] m_directions;
        private Vector3 m_chargePoint;
        private bool m_descending;
        private Vector3 m_currentMovementDirection;

        public void Setup(Drone a_drone)
        {
            m_drone = a_drone;
            m_droneT = a_drone.transform;
            m_battery = a_drone.BatteryComponent;
            m_state = DroneMovementState.IdleOnGround;

            InitializeDirections();
            InitializeBehaviors();
        }

        public void Init(Transform playerT)
        {
            m_playerT = playerT;
            
            if (m_chaseBehavior != null)
                m_chaseBehavior.Init(playerT);
            
            if (m_rotateBehavior != null)
                m_rotateBehavior.Init(playerT);
            
            if (m_randomBehavior != null)
                m_randomBehavior.Init(0.5f);
        }

        private void InitializeDirections()
        {
            m_directions = new Vector3[m_directionCount];
            float angleStep = 360f / m_directionCount;
            
            for (int i = 0; i < m_directionCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                m_directions[i] = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)).normalized;
            }
        }

        private void InitializeBehaviors()
        {
            if (m_chaseBehavior != null)
                m_chaseBehavior.Setup(m_directions);
            
            if (m_randomBehavior != null)
                m_randomBehavior.Setup(m_directions);
            
            if (m_focusBehavior != null)
                m_focusBehavior.Setup(m_directions);
            
            if (m_rotateBehavior != null)
                m_rotateBehavior.Setup(m_directions);
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
        }

        private void UpdateIdle()
        {
            m_currentMovementDirection = Vector3.zero;
            if (m_battery != null && !m_battery.IsRecharging)
                m_battery.StartRecharge(m_droneT.position);
        }

        private void UpdateTakeOff()
        {
            Vector3 target = new Vector3(m_playerT.position.x, m_playerT.position.y + m_hoverHeight, m_playerT.position.z);
            
            if (m_focusBehavior != null)
            {
                m_focusBehavior.Init(target);
                Vector3 moveDirection = ComputeMovementDirection(m_focusBehavior);
                m_currentMovementDirection = moveDirection;
                
                if (moveDirection.magnitude > 0.01f)
                {
                    Vector3 horizontalMove = new Vector3(moveDirection.x, 0f, moveDirection.z) * m_moveSpeed * Time.deltaTime;
                    Vector3 verticalTarget = new Vector3(m_droneT.position.x, target.y, m_droneT.position.z);
                    m_droneT.position += horizontalMove;
                    m_droneT.position = Vector3.Lerp(m_droneT.position, verticalTarget, m_transitionSpeed * Time.deltaTime);
                }
                else
                {
                    m_currentMovementDirection = Vector3.zero;
                    m_droneT.position = Vector3.Lerp(m_droneT.position, target, m_transitionSpeed * Time.deltaTime);
                }
            }
            else
            {
                Vector3 dirToTarget = (target - m_droneT.position).normalized;
                m_currentMovementDirection = new Vector3(dirToTarget.x, 0f, dirToTarget.z);
                m_droneT.position = Vector3.Lerp(m_droneT.position, target, m_transitionSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(m_droneT.position, target) < 0.1f)
            {
                SetState(DroneMovementState.FollowingPlayer);
            }
        }

        private void UpdateFollowing()
        {
            if (m_chaseBehavior == null || m_randomBehavior == null) return;

            // Calculer la position cible autour du joueur
            Vector3 playerPos = m_playerT.position;
            Vector3 targetPos = new Vector3(playerPos.x, playerPos.y + m_hoverHeight, playerPos.z);
            
            // Vérifier la distance horizontale du drone au joueur
            Vector3 dronePosHorizontal = new Vector3(m_droneT.position.x, 0f, m_droneT.position.z);
            Vector3 playerPosHorizontal = new Vector3(playerPos.x, 0f, playerPos.z);
            float distanceToPlayer = Vector3.Distance(dronePosHorizontal, playerPosHorizontal);
            
            // Combiner chase et random behaviors
            float[] chaseWeights = m_chaseBehavior.Compute();
            float[] randomWeights = m_randomBehavior.Compute();
            
            // Si le drone dépasse le périmètre, ajouter un poids pour le ramener vers le joueur
            float[] perimeterWeights = null;
            if (distanceToPlayer > m_followRadius && m_focusBehavior != null)
            {
                Vector3 returnPos = playerPosHorizontal + (dronePosHorizontal - playerPosHorizontal).normalized * m_followRadius;
                returnPos.y = targetPos.y;
                m_focusBehavior.Init(returnPos);
                perimeterWeights = m_focusBehavior.Compute();
            }
            
            // Combiner les poids (addition pondérée)
            float[] combinedWeights = new float[m_directions.Length];
            for (int i = 0; i < m_directions.Length; i++)
            {
                combinedWeights[i] = chaseWeights[i] * 0.7f + randomWeights[i] * 0.3f;
                
                // Si le drone dépasse le périmètre, ajouter un poids fort pour le ramener
                if (perimeterWeights != null && i < perimeterWeights.Length)
                {
                    float perimeterWeight = perimeterWeights[i] * (distanceToPlayer / m_followRadius - 1f);
                    combinedWeights[i] = Mathf.Max(combinedWeights[i], perimeterWeight * 2f);
                }
            }

            // Calculer la direction de mouvement
            Vector3 moveDirection = Vector3.zero;
            float totalWeight = 0f;
            
            for (int i = 0; i < m_directions.Length; i++)
            {
                if (combinedWeights[i] > 0f)
                {
                    moveDirection += m_directions[i] * combinedWeights[i];
                    totalWeight += combinedWeights[i];
                }
            }
            
            if (totalWeight > 0f)
                moveDirection /= totalWeight;

            // Stocker la direction de mouvement pour le composant visuel
            m_currentMovementDirection = moveDirection;

            // Appliquer le mouvement horizontal
            Vector3 horizontalMove = new Vector3(moveDirection.x, 0f, moveDirection.z) * m_moveSpeed * Time.deltaTime;
            Vector3 newPos = m_droneT.position + horizontalMove;
            
            // Vérifier que le nouveau mouvement ne dépasse pas le périmètre
            Vector3 newPosHorizontal = new Vector3(newPos.x, 0f, newPos.z);
            float newDistanceToPlayer = Vector3.Distance(newPosHorizontal, playerPosHorizontal);
            
            if (newDistanceToPlayer > m_followRadius)
            {
                // Limiter la position au périmètre
                Vector3 directionToPlayer = (playerPosHorizontal - newPosHorizontal).normalized;
                newPosHorizontal = playerPosHorizontal - directionToPlayer * m_followRadius;
                newPos.x = newPosHorizontal.x;
                newPos.z = newPosHorizontal.z;
            }
            
            // Maintenir la hauteur de vol
            newPos.y = Mathf.Lerp(m_droneT.position.y, targetPos.y, m_transitionSpeed * Time.deltaTime);
            m_droneT.position = newPos;
        }

        private void UpdateReturning()
        {
            if (m_battery == null || m_focusBehavior == null) return;

            if (!m_descending)
            {
                Vector3 chargePos = m_battery.LastChargePoint;
                chargePos.y = m_playerT.position.y + m_hoverHeight;
                
                m_focusBehavior.Init(chargePos);
                Vector3 moveDirection = ComputeMovementDirection(m_focusBehavior);
                m_currentMovementDirection = moveDirection;
                
                if (moveDirection.magnitude > 0.01f)
                {
                    Vector3 horizontalMove = new Vector3(moveDirection.x, 0f, moveDirection.z) * m_moveSpeed * Time.deltaTime;
                    m_droneT.position += horizontalMove;
                    
                    // Maintenir la hauteur
                    Vector3 heightTarget = new Vector3(m_droneT.position.x, chargePos.y, m_droneT.position.z);
                    m_droneT.position = Vector3.Lerp(m_droneT.position, heightTarget, m_transitionSpeed * Time.deltaTime);
                }
                else
                {
                    Vector3 dirToCharge = (chargePos - m_droneT.position).normalized;
                    m_currentMovementDirection = new Vector3(dirToCharge.x, 0f, dirToCharge.z);
                    m_droneT.position = Vector3.MoveTowards(m_droneT.position, chargePos, m_moveSpeed * Time.deltaTime);
                }

                if (Vector3.Distance(new Vector3(m_droneT.position.x, 0f, m_droneT.position.z), 
                    new Vector3(chargePos.x, 0f, chargePos.z)) < 0.2f)
                {
                    m_chargePoint = m_battery.LastChargePoint;
                    m_descending = true;
                    SetState(DroneMovementState.Landing);
                }
            }
        }

        private void UpdateLanding()
        {
            if (m_focusBehavior == null) return;

            Vector3 target = m_chargePoint + Vector3.up * m_landingHeight;
            m_focusBehavior.Init(target);
            Vector3 moveDirection = ComputeMovementDirection(m_focusBehavior);
            m_currentMovementDirection = moveDirection;
            
            if (moveDirection.magnitude > 0.01f)
            {
                Vector3 horizontalMove = new Vector3(moveDirection.x, 0f, moveDirection.z) * m_moveSpeed * Time.deltaTime;
                m_droneT.position += horizontalMove;
            }
            else
            {
                m_currentMovementDirection = Vector3.zero;
            }
            
            // Atterrissage vertical
            Vector3 verticalTarget = new Vector3(m_droneT.position.x, target.y, m_droneT.position.z);
            m_droneT.position = Vector3.MoveTowards(m_droneT.position, verticalTarget, m_transitionSpeed * Time.deltaTime);
            
            if (Vector3.Distance(m_droneT.position, target) < 0.05f)
            {
                m_battery.StartRecharge(m_chargePoint);
                m_descending = false;
                SetState(DroneMovementState.IdleOnGround);
            }
        }

        private Vector3 ComputeMovementDirection(MovementBehavior behavior)
        {
            if (behavior == null) return Vector3.zero;

            float[] weights = behavior.Compute();
            Vector3 direction = Vector3.zero;
            float totalWeight = 0f;

            for (int i = 0; i < weights.Length && i < m_directions.Length; i++)
            {
                if (weights[i] > 0f)
                {
                    direction += m_directions[i] * weights[i];
                    totalWeight += weights[i];
                }
            }

            if (totalWeight > 0f)
                direction /= totalWeight;

            return direction.normalized;
        }

        public void Launch()
        {
            if (m_state == DroneMovementState.IdleOnGround)
                SetState(DroneMovementState.TakeOff);
        }

        public Vector3 GetMovementDirection()
        {
            return m_currentMovementDirection;
        }

        public float MoveSpeed => m_moveSpeed;
    }
}