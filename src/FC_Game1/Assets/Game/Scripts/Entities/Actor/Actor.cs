using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class Actor : Entity
    {
        #region inspector
        [SerializeField] private string m_id; // Unique par Actor (Player, Npc, Vendor)
        public string Id => m_id;
        public Actor_HealthComponent HealthComponent => m_healthComponent;
        [SerializeField] private Actor_HealthComponent m_healthComponent;

        #endregion

        #region properties

        public Rigidbody Rigidbody => m_rigidbody;
        private Rigidbody m_rigidbody;

        #endregion

        public override void Setup(GameContextScriptable a_context)
        {
            base.Setup(a_context);
            m_rigidbody = GetComponent<Rigidbody>();
            // m_healthComponent.Setup(this, );
            int myMaxHP = ResolveMyMaxHP(a_context);
            m_healthComponent.Setup(this, new FCTools.FloatableParam(myMaxHP));
        }
        public override void Init(GameState state)
        {
            base.Init(state);

            ActorState myState = ResolveMyState(state);
            if (myState == null)
            {
                Debug.LogWarning($"No ActorState found for {Id}, using defaults.");
                m_healthComponent.Init(m_healthComponent.MaxHealth);
                return;
            }

            // Points de vie
            m_healthComponent.Init(new FCTools.FloatableParam(myState.CurrentHp));

            // Position
            transform.position = myState.Position;
        }

        protected virtual ActorState ResolveMyState(GameState state)
        {
            if (Id == "Player")
                return state.Player;

            return state.Npcs.FirstOrDefault(n => n.Id == Id);
        }
        protected virtual int ResolveMyMaxHP(GameContextScriptable a_context)
        {
            if (Id == "Player")
                return a_context.PlayerMaxHealth;

            return a_context.NpcsMaxHealth;
        }

    }
}
