using UnityEngine;

namespace Game
{
    public class Interactable : Entity
    {
        #region properties
        private Interactable_TriggerComponent m_triggerComponent;
        public Interactable_TriggerComponent TriggerComponent => m_triggerComponent;
        #endregion

        public override void Setup(GameContextScriptable a_gameContext)
        {
            base.Setup(a_gameContext);
            m_triggerComponent.Setup(this);
        }
        public override void Init(GameState a_gameState)
        {
            base.Init(a_gameState);
            m_triggerComponent.Init();
        }

        protected virtual void OnDestroy()
        {
            m_triggerComponent.DoOnDestroy();
        }
    }
}
