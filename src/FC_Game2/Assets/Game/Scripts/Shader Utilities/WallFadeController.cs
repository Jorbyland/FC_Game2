using UnityEngine;

namespace Game
{

    [ExecuteAlways]
    public class WallFadeController : MonoBehaviour
    {
        [SerializeField] Material targetMaterial; // material créé à partir du ShaderGraph
        [SerializeField] Transform player;

        void Update()
        {
            if (targetMaterial == null || player == null) return;
            targetMaterial.SetVector("_PlayerPosWS", player.position);
        }
    }
}
