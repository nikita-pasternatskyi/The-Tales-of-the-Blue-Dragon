using MP.Game.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MP.Game.Objects.Tutorial
{

    public class Tutorial : DynamicUI
    {
        [SerializeField] private TutorialView _tutorialViewPrefab;
        [SerializeField] private InputActionReference _inputToListenTo;
        [SerializeField] private bool _hideByTimer = false;
        [SerializeField, Tooltip("Works only if Hide by Timer is true, sets how long should the tutorial last")]
        private float _showTimer = 1.0f;
        [SerializeField] private string _text;
        [SerializeField] private float _timeToWaitUntilCompletion;
        private TutorialView _tutorialView;
        private float _inputTimer;
        private float _timer;

        public UnityEvent Activated;
        public UnityEvent Completed;

        private bool _completed;
        private bool _activated;

        private void OnTriggerEnter(Collider other)
        {
            if (_completed)
                return;
            if (_activated)
                return;
            _tutorialView = Instantiate(_tutorialViewPrefab, s_CanvasInstance.transform);
            _tutorialView.ButtonHint.Button = _inputToListenTo;
            _tutorialView.Text.text = _text;
            if (other.GetComponent<Player.Scripts.Player>() != null)
            {
                _activated = true;
                Activated?.Invoke();
                _tutorialView.Animator.SetTrigger("Activated");
            }
        }

        private void Update()
        {
            if (!_activated)
                return;

            if(_hideByTimer)
            {
                _timer += Time.deltaTime;
                if(_timer >= _showTimer)
                {
                    Complete();
                }
            }

            if (_inputToListenTo.action.IsPressed())
            {
                _inputTimer += Time.deltaTime;
                if (_inputTimer >= _timeToWaitUntilCompletion)
                {
                    Complete();
                }
            }
        }

        private void Complete()
        {
            _completed = true;
            Completed?.Invoke();
            _tutorialView.Animator.SetTrigger("Completed");
        }
    }
}
