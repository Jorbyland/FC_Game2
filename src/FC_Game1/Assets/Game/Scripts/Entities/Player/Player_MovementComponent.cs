using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CharacterController))]
    public class Player_MovementComponent : MonoBehaviour
    {
        private Player m_player;
        private CharacterController m_controller;

        [Header("Movement")]
        [SerializeField] private float m_moveSpeed = 5f;
        [SerializeField] private float m_sprintMultiplier = 2f;
        [SerializeField] private float m_gravity = -9.81f;
        [SerializeField] private float m_groundedGravity = -2f;

        private bool m_sprint;
        private Vector3 m_velocity;
        private Vector3 m_moveInput;
        private Transform m_playerVisual;

        public void Setup(Player a_player)
        {
            m_player = a_player;
            m_playerVisual = m_player.Visual;
            m_controller = GetComponent<CharacterController>();
        }

        public void Init()
        {
            m_velocity = Vector3.zero;
        }

        public void DoUpdate()
        {
            ApplyGravity();
            MovePlayer();
        }

        public void Move(Vector3 a_moveInput)
        {
            m_moveInput = a_moveInput;
        }

        public void Sprint(bool a_sprintInput)
        {
            m_sprint = a_sprintInput;
        }

        private void MovePlayer()
        {
            // Vector3 move = new Vector3(m_moveInput.x, 0f, m_moveInput.z);
            // if (move.sqrMagnitude > 1f)
            //     move.Normalize();

            // float currentSpeed = m_moveSpeed * (m_sprint ? m_sprintMultiplier : 1f);
            // Vector3 worldMove = transform.TransformDirection(move) * currentSpeed;

            // m_controller.Move((worldMove + m_velocity) * Time.deltaTime);

            // Movement dans le sens de la camera
            Transform cam = CameraManager.Instance.CameraTransform;
            if (cam == null) return;

            // Direction relative à la caméra
            Vector3 forward = cam.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = cam.right;
            right.y = 0f;
            right.Normalize();

            Vector3 moveDir = (right * m_moveInput.x + forward * m_moveInput.z);
            if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

            float speed = m_moveSpeed * (m_sprint ? m_sprintMultiplier : 1f);
            m_velocity = moveDir * speed;

            m_controller.SimpleMove(m_velocity);

            // Orientation du joueur
            if (moveDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                m_playerVisual.rotation = Quaternion.Slerp(m_playerVisual.rotation, targetRot, Time.deltaTime * 10f);
            }
        }

        private void ApplyGravity()
        {
            if (m_controller.isGrounded && m_velocity.y < 0f)
                m_velocity.y = m_groundedGravity;
            else
                m_velocity.y += m_gravity * Time.deltaTime;
        }
    }
}