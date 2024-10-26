using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts
{
    public class PlayerDialogue : MonoBehaviour
    {
        [SerializeField] private Dialogue.DialogueEvent _dialogueEnterEvent;
        [SerializeField] private Dialogue.DialogueEvent _dialogueExitEvent;
        [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;

        public UnityEvent DialogueEntered;
        public UnityEvent DialogueExited;

        private void OnEnable()
        {
            _dialogueEnterEvent.GenericEvent += OnDialogueEntered;
            _dialogueExitEvent.Event += OnDialogueExited;
        }

        private void OnDialogueExited()
        {
            DialogueExited?.Invoke();
        }

        private void OnDialogueEntered(GameObject obj)
        {
            DialogueEntered?.Invoke();
            _cinemachineTargetGroup.m_Targets[1].target = obj.transform;
        }

        private void OnDisable()
        {
            _dialogueEnterEvent.GenericEvent -= OnDialogueEntered;
            _dialogueExitEvent.Event -= OnDialogueExited;
        }
    }
}
