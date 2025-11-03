using UnityEngine;

namespace Game
{
    [ExecuteAlways]
    public class CameraVisibilityCone : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float coneWidth = 3f;
        [SerializeField] private float fadeSmoothness = 1f;
        [SerializeField] private Material[] targetMaterials; // matériaux utilisant ton shader

        private Camera cam;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            if (!player || targetMaterials == null) return;

            Vector3 camPos = cam.transform.position;
            Vector3 playerPos = player.position;

            foreach (var mat in targetMaterials)
            {
                if (mat == null) continue;

                mat.SetVector("_PlayerPos", playerPos);
                mat.SetVector("_CameraPos", camPos);
                mat.SetFloat("_ConeWidth", coneWidth);
                mat.SetFloat("_FadeSmoothness", fadeSmoothness);
            }
        }
    }
}
