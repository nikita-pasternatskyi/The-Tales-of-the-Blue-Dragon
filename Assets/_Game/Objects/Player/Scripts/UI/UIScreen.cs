using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts.UI
{
    public class UIScreen : MonoBehaviour
    {
        public UnityEvent Showed;
        public UnityEvent Hidden;

        public void Hide()
        {
            Hidden?.Invoke();
        }

        public void Show()
        {
            Showed?.Invoke();
        }
    }
}
