using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Config/GameContext")]
    public class GameContextScriptable : ScriptableObject
    {
        public string PlayerId = "Player";
        public Vector3 PlayerStartPosition = new Vector3(0, 0, 0);
        public int PlayerInventorySize;
        public int PlayerMaxHealth;
        public int PlayerStartMoney;

        public int NpcsMaxHealth;

        public List<ItemScriptable> ItemScriptables;
        public List<NpcScriptable> NpcScriptables;
        public List<NpcVendorScriptable> NpcVendorScriptables;

        public NpcVendorScriptable GetNpcVendorScriptable(string a_id)
        {
            return NpcVendorScriptables.FirstOrDefault(n => n.Id == a_id);
        }
        public NpcScriptable GetNpcScriptable(string a_id)
        {
            return NpcScriptables.FirstOrDefault(n => n.Id == a_id);
        }
        public ItemScriptable GetItemScriptable(string a_id)
        {
            return ItemScriptables.FirstOrDefault(n => n.Id == a_id);
        }
    }
}
