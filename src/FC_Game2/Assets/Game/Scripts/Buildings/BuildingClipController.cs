using UnityEngine;

namespace Game
{
    [ExecuteAlways]
    public class BuildingClipController : MonoBehaviour
    {
        [Header("Clip Settings")]
        [SerializeField] private Transform m_player;
        [SerializeField] private float m_clipRadius = 3f;
        [SerializeField] private string m_clipPositionProperty = "_ClipCenter";
        [SerializeField] private string m_clipRadiusProperty = "_ClipRadius";

        private static readonly int ClipCenterID = Shader.PropertyToID("_ClipCenter");
        private static readonly int ClipRadiusID = Shader.PropertyToID("_ClipRadius");

        private void Update()
        {
            if (m_player == null)
                return;

            Vector3 center = m_player.position;

            // Envoi des paramètres globaux accessibles à tous les matériaux
            Shader.SetGlobalVector(ClipCenterID, center);
            Shader.SetGlobalFloat(ClipRadiusID, m_clipRadius);
        }
    }
}