using MP.Game.Objects.Interactable;
using TMPro;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    public class PlayerInteractionPromptUI : MonoBehaviour
    {
        public Animator Animator;
        public string AppearAnimationTrigger = "Appear";
        public string DisappearAnimationTrigger = "Disappear";
        public TextMeshProUGUI PromptText;

        public void UpdatePrompt(IInteractable interactable)
        {
            if (interactable == null)
            {
                Animator.SetTrigger(DisappearAnimationTrigger);
                return;
            }
            PromptText.text = interactable.InteractionName;
            Animator.SetTrigger(AppearAnimationTrigger);
        }
    }
}
