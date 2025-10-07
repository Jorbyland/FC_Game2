
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PlayerState
    {
        public int Money;
        public List<InventorySlotData> Inventory;
    }

    [System.Serializable]
    public class ActorState
    {
        public string Id;       // identifiant unique (Player, Npc_X, etc.)
        public int CurrentHp;
        public Vector3 Position;
    }
    [System.Serializable]
    public class InventorySlotState
    {
        public ItemScriptable ItemSO;
        public int Quantity;
        public int Price; // utilisé seulement pour Vendor
    }
    [System.Serializable]
    public class InventoryState
    {
        public List<InventorySlotState> Slots = new List<InventorySlotState>();
    }

    [System.Serializable]
    public class GameState
    {
        public ActorState Player;
        public InventoryState PlayerInventory = new InventoryState();
        public int PlayerWallet = 200;

        public List<ActorState> Npcs;
        public Dictionary<string, InventoryState> NpcVendors = new Dictionary<string, InventoryState>();
        public float WorldTime;
    }
}
