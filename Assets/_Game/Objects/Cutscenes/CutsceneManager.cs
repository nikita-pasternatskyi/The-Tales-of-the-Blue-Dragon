using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Assets._Game.Objects.Cutscenes
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent _cutsceneOnEvent;
        [SerializeField] private ScriptableEvent _cutsceneOffEvent;
        [SerializeField] private Animator _cutsceneBars;

        private void OnEnable()
        {
            _cutsceneOnEvent.Event += OnCutscene;
            _cutsceneOffEvent.Event += OffCutscene;
        }

        private void OffCutscene()
        {
            _cutsceneBars.SetTrigger("Hide");
        }

        private void OnCutscene()
        {
            _cutsceneBars.SetTrigger("Activate");
        }

        private void OnDisable()
        {
            _cutsceneOnEvent.Event -= OnCutscene;
            _cutsceneOffEvent.Event -= OffCutscene;
        }
    }
}
