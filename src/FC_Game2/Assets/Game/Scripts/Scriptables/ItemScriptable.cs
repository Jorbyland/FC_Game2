using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Config/ItemDefinition")]
    public class ItemScriptable : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public string Description;
        public int BasePrice;
        public int MaxStack = 1;
        public Sprite Icon;
    }
}
