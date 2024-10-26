using MP.Game.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MP.Game.Objects.Dialogue
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string Text;
        public float ScrollSpeed;
        public bool Skippable;
    }

    public class Dialogue : DynamicUI
    {
        [SerializeField] private InputActionReference[] _actionsThatContinueDialogue;
        [SerializeField] private GameObject _head;
        [SerializeField] private DialogueEvent _dialogueInitatedEvent;
        [SerializeField] private DialogueEvent _dialogueEndedEvent;
        [SerializeField] private DialogueView _dialogueUIViewPrefab;
        [SerializeField] private DialogueLine[] _dialogueLines;
        [SerializeField] private bool _canRestart;

        public UnityEvent DialogueStarted;
        public UnityEvent DialogueProgressed;
        public UnityEvent DialogueCompleted;

        private bool _active;
        private int _currentDialogueLine;
        private DialogueView _pooledDialogueView;

        private void Subscribe()
        {
            foreach (var item in _actionsThatContinueDialogue)
            {
                item.action.performed += OnButtonPressed;
            }
        }

        private void Unsubscribe()
        {
            foreach (var item in _actionsThatContinueDialogue)
            {
                item.action.performed -= OnButtonPressed;
            }
        }

        private void OnButtonPressed(InputAction.CallbackContext obj)
        {
            if (_active)
            {
                if (_pooledDialogueView.Showing)
                    TriggerDialogue();
            }
        }

        public void TriggerDialogue()
        {
            void ShowDialogue()
            {
                _pooledDialogueView.Show(_dialogueLines[_currentDialogueLine]);
            }

            if (!_active)
            {
                _currentDialogueLine = 0;
                Subscribe();
                _active = true;
            }

            if(!_pooledDialogueView)
            {
                _pooledDialogueView = Instantiate(_dialogueUIViewPrefab, s_CanvasInstance.transform);
            }

            if (!_pooledDialogueView.Showing)
            {
                _dialogueInitatedEvent.Invoke(_head);
                DialogueStarted?.Invoke();
                ShowDialogue();
                return;
            }

            if(_pooledDialogueView.LineCompleted)
            {
                _currentDialogueLine++;
                if (_currentDialogueLine < _dialogueLines.Length) //if we still have more lines to show
                {
                    ShowDialogue();
                    return;
                }
                //no lines left
                _pooledDialogueView.Hide();
                _dialogueEndedEvent.Invoke();
                DialogueCompleted?.Invoke();
                _currentDialogueLine = 0;
                _active = false;
                Unsubscribe();
                return;
            }

            if(_pooledDialogueView.Showing && !_pooledDialogueView.LineCompleted && _dialogueLines[_currentDialogueLine].Skippable)
                _pooledDialogueView.CompleteLine();

        }
    }
}
