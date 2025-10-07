using UnityEngine;

namespace Game
{
    public interface IInteractable
    {
        void Interact(Entity a_interactor);
        bool CanInteract(Entity a_interactor);
        string GetInteractionPrompt();
    }
}
