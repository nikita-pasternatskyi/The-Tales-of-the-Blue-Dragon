using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Interactable
{
    public interface IInteractable
    {
        string InteractionName { get; }
        bool RequireButtonPrompt { get; }
        float ActiveDotProductRange { get; }
        float CloseDetectionRange { get; }
        bool Active { get; }
        void Interact(GameObject sender);
    }
}
