using UnityEngine;
using UnityEngine.UI;

namespace MP.Game.Utils
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageScroller : MonoBehaviour
    {
        [SerializeField] private Vector2 _speed;
        [SerializeField] private bool _unscaledTime = true;
        private RawImage _rawImage;

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            var rect = _rawImage.uvRect;
            var dt = _unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            rect.x += _speed.x * dt;
            rect.y += _speed.y * dt;

            _rawImage.uvRect = rect;
        }
    }
}
