using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Dialogue
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textField;
        public UnityEvent Shown;
        public UnityEvent Hidden;

        public bool Showing;
        private Coroutine _coroutine;

        public bool LineCompleted { get; private set; }
        private DialogueLine _currentDialogueLine;


        public void Show(DialogueLine line)
        {
            LineCompleted = false;
            _currentDialogueLine = line;
            Shown?.Invoke();
            Showing = true;
            _coroutine = StartCoroutine(AnimateText());
        }

        public void Hide()
        {
            Showing = false;
            Hidden?.Invoke();
        }

        public void CompleteLine()
        {           
            LineCompleted = true;
            _textField.text = _currentDialogueLine.Text;
            StopCoroutine(_coroutine);
        }

        private IEnumerator AnimateText()
        {
            for(int i = 0; i < _currentDialogueLine.Text.Length; i++)
            {
                _textField.text = _currentDialogueLine.Text.Substring(0, i);
                yield return new WaitForSeconds(_currentDialogueLine.ScrollSpeed);
            }
            CompleteLine();
        }
    }
}
