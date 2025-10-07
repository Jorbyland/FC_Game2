using System.Collections.Generic;
using Main;
using UnityEngine;

namespace Game
{
    public class GameInitializer : MonoBehaviour
    {
        #region properties
        [SerializeField] private GameContextScriptable m_gameContext;// globals resources
        [SerializeField] private List<Entity> m_entitiesRef;
        private List<IGameComponent> m_entitiesComponents;

        private GameState m_gameState;
        #endregion

        void Awake()
        {
            m_entitiesComponents = new List<IGameComponent>();
            foreach (var c in m_entitiesRef)
            {
                if (c is IGameComponent comp) m_entitiesComponents.Add(comp);
            }

            foreach (var comp in m_entitiesComponents)
                comp.Setup(m_gameContext);

            m_gameState = LoadOrCreateGameState();
            foreach (var comp in m_entitiesComponents)
                comp.Init(m_gameState);
        }

        void Start()
        {
            GUIManager.Instance.Init(AppContext.A.Player, m_gameContext, m_gameState);
        }

        private GameState LoadOrCreateGameState()
        {
            // charger une sauvegarde ou créer un état vierge
            GameState gameState = CreateNewGameState();
            return gameState;
        }

        private GameState CreateNewGameState()
        {
            GameState gameState = new GameState();
            gameState.Player = new ActorState()
            {
                Id = m_gameContext.PlayerId,
                CurrentHp = m_gameContext.PlayerMaxHealth,
                Position = m_gameContext.PlayerStartPosition
            };

            gameState.Npcs = new List<ActorState>();
            gameState.NpcVendors = new Dictionary<string, InventoryState>();
            foreach (var c in m_entitiesRef)
            {
                if (c.TryGetComponent<Npc>(out var npc))
                {
                    ActorState actorState = new()
                    {
                        Id = npc.Id,
                        CurrentHp = m_gameContext.NpcsMaxHealth,
                        Position = npc.transform.position,
                    };
                    gameState.Npcs.Add(actorState);
                }

                if (c.TryGetComponent<VendorNpc>(out var vendor))
                {
                    InventoryState inventoryState = new()
                    {
                        Slots = new List<InventorySlotState>(),
                    };
                    gameState.NpcVendors.Add(vendor.Id, inventoryState);
                }
            }
            return gameState;
        }


    }
}
