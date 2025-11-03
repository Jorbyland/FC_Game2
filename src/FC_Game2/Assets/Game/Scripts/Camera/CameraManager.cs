using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [DefaultExecutionOrder(-100)]
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager s_instance;
        public static CameraManager Instance => s_instance;

        [Header("Dependencies")]
        [SerializeField] private CinemachineBrain m_brain;
        [SerializeField] private CinemachineBlenderSettings m_jumpBlend;
        [SerializeField] private CinemachineBlenderSettings m_smoothBlend;

        [Header("Camera References")]
        [SerializeField] private CinemachineCamera m_exteriorCamera;
        [SerializeField] private CinemachineCamera m_interiorCamera;


        [Header("Components")]
        [SerializeField] private CameraObstacleDetector m_obstacleDetector;
        public CameraObstacleDetector ObstacleDetector => m_obstacleDetector;

        [Header("Auto-align")]
        [SerializeField] private Transform m_player;
        [SerializeField] private float m_mouseSensitivity = 0.2f;
        [SerializeField] private float m_returnSpeed = 1.5f;
        [SerializeField] private float m_autoAlignDelay = 2f; // secondes avant que la caméra se recale

        #region properties
        private List<CinemachineCamera> m_virtualCameras = new List<CinemachineCamera>();
        private CinemachineCamera m_currentCamera;
        private CinemachineOrbitalFollow m_orbitalFollow;
        private CinemachinePositionComposer m_positionComposer;
        public CinemachinePanTilt m_panTilt;

        private float m_timeSinceLastLookInput = 0f;
        private Vector2 m_lookInput;
        private Camera m_camera;
        #endregion

        private void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;
            Setup();
        }

        public void Setup()
        {
            m_virtualCameras.Clear();

            if (m_exteriorCamera != null) Register(m_exteriorCamera);
            if (m_interiorCamera != null) Register(m_interiorCamera);

            if (m_brain == null && Camera.main != null)
                m_brain = Camera.main.GetComponent<CinemachineBrain>();
            m_camera = Camera.main;

            SetActiveCamera(m_exteriorCamera, true); // start with exterior
        }

        public void Register(CinemachineCamera cam)
        {
            if (cam != null && !m_virtualCameras.Contains(cam))
                m_virtualCameras.Add(cam);
        }

        public void Unregister(CinemachineCamera cam)
        {
            if (cam != null)
                m_virtualCameras.Remove(cam);
        }

        public void SetActiveCamera(CinemachineCamera target, bool jumpCut = false)
        {
            if (target == null) return;

            if (m_brain != null)
                m_brain.CustomBlends = jumpCut ? m_jumpBlend : m_smoothBlend;

            m_currentCamera = target;
            m_orbitalFollow = m_currentCamera != null ? m_currentCamera.GetComponent<CinemachineOrbitalFollow>() : null;
            m_panTilt = m_currentCamera != null ? m_currentCamera.GetComponent<CinemachinePanTilt>() : null;
            m_positionComposer = m_currentCamera != null ? m_currentCamera.GetComponent<CinemachinePositionComposer>() : null;

            foreach (var cam in m_virtualCameras)
                cam.Priority = (cam == target) ? 10 : 0;
        }

        public void SwitchToExterior(bool jumpCut = false) => SetActiveCamera(m_exteriorCamera, jumpCut);
        public void SwitchToInterior(bool jumpCut = false) => SetActiveCamera(m_interiorCamera, jumpCut);

        public bool IsInteriorActive => m_currentCamera == m_interiorCamera;

        void LateUpdate()
        {
            // if (m_orbitalFollow == null || m_player == null)
            //     return;

            // m_timeSinceLastLookInput += Time.deltaTime;

            // // --- Rotation manuelle ---
            // if (m_lookInput.sqrMagnitude > 0.0001f)
            // {
            //     m_orbitalFollow.HorizontalAxis.Value += m_lookInput.x * m_mouseSensitivity;
            //     // m_orbitalFollow.VerticalAxis.Value -= m_lookInput.y * m_mouseSensitivity;
            //     m_orbitalFollow.VerticalAxis.Value = Mathf.Clamp(m_orbitalFollow.VerticalAxis.Value, -30f, 60f);
            // }

            // --- Auto-alignement retardé ---
            // if (m_timeSinceLastLookInput > m_autoAlignDelay)
            // {
            //     Vector3 playerScreenPos = m_camera.WorldToViewportPoint(m_player.position);
            //     float centerOffset = Vector2.Distance(new Vector2(playerScreenPos.x, playerScreenPos.y),
            //                                           new Vector2(0.5f, 0.5f));

            //     if (centerOffset > 0.05f)
            //     {
            //         float playerYaw = m_player.eulerAngles.y;
            //         float currentAngle = m_orbitalFollow.HorizontalAxis.Value;
            //         float smoothAngle = Mathf.LerpAngle(currentAngle, playerYaw, Time.deltaTime * m_returnSpeed);
            //         m_orbitalFollow.HorizontalAxis.Value = smoothAngle;
            //     }
            // }
        }
        public void OnLook(InputAction.CallbackContext ctx)
        {
            m_lookInput = ctx.ReadValue<Vector2>();
            if (m_lookInput.sqrMagnitude > 0.0001f)
                m_timeSinceLastLookInput = 0f;
        }

        public Transform CameraTransform => m_currentCamera != null ? m_currentCamera.gameObject.transform : null;
    }
}