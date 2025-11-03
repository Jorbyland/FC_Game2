using UnityEngine;

namespace Game
{
    public class Npc : Actor, IInteractable
    {
        #region properties
        protected bool canInteract = true;
        #endregion

        public override void Setup(GameContextScriptable a_gameContext)
        {
            base.Setup(a_gameContext);
            gameObject.name = Id;
        }
        public override void Init(GameState a_gameState)
        {
            base.Init(a_gameState);
        }

        public virtual void Interact(Entity interactor)
        {
            Debug.Log($"Interaction of : {gameObject.name}");
            canInteract = false;
            OnInteractionCompleted();
        }

        public virtual bool CanInteract(Entity interactor)
        {
            return canInteract;
        }

        public string GetInteractionPrompt() => "Parler";

        public virtual void OnInteractionCompleted()
        {
            canInteract = true;
        }
    }
}
