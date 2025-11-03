using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BuildingColorPreset", menuName = "Game/Building Color Preset")]
    public class BuildingColorPreset : ScriptableObject
    {
        public string presetName;
        [Tooltip("Index palette par défaut pour les éléments extérieurs")]
        public int exteriorIndex = 0;
        [Tooltip("Index palette par défaut pour les éléments intérieurs")]
        public int interiorIndex = 1;
        [Tooltip("Optionnel : override par zone")]
        public int floorIndex = 2;
    }
}
