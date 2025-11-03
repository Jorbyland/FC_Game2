using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BuildingPrefabLibrary", menuName = "Game/Building Prefab Library")]
    public class BuildingPrefabLibraryScriptable : ScriptableObject
    {
        [Header("Floors")]
        public GameObject[] floors;

        [Header("Walls / Windows / Doors")]
        public GameObject[] walls;

        [Header("Roofs")]
        public GameObject[] roofs;
    }
}
