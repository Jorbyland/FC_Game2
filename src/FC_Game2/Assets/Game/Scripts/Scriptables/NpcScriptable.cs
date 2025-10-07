using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Config/NpcDefinition")]
    public class NpcScriptable : ScriptableObject

    {
        public string Id;           // ex: "Npc_Marchand_001"
        public string DisplayName;
        public int BaseMaxHp;
    }
 
}
