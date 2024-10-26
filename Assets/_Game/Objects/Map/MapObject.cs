using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Map
{
    public class MapObject : MonoBehaviour
    {
        [SerializeField] private Color _targetGraphicColor;
        [SerializeField] private Sprite _targetGraphic;
        [SerializeField, Layer] private int _miniMapLayer = 9;
        [SerializeField] private Vector3 _localPositionOffset = Vector3.zero;
        [SerializeField] private Vector3 _localRotation = new Vector3(90, 0, 0);
        [SerializeField] private Vector3 _scale = Vector3.one * 4;
        [SerializeField] private bool _addToMapFromStart = true;
        private GameObject _mapObject;

        private static string _suffix = "_map";

        public void RemoveFromMap()
        {
            if (_mapObject != null)
            {
                _mapObject.SetActive(false);
            }
        }

        public void AddToMap()
        {
            if (_mapObject == null)
            {
                GameObject mapIcon = new GameObject(name + _suffix, typeof(SpriteRenderer));
                mapIcon.transform.parent = transform;
                mapIcon.transform.localEulerAngles = _localRotation;
                mapIcon.transform.localPosition = _localPositionOffset;
                mapIcon.transform.localScale = _scale;
                mapIcon.layer = _miniMapLayer;
                var spriteRenderer = mapIcon.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = _targetGraphic;
                spriteRenderer.color = _targetGraphicColor;
                spriteRenderer.sprite = _targetGraphic;
                _mapObject = mapIcon;
            }

            _mapObject.SetActive(true);
        }

        private void Start()
        {
            if (_addToMapFromStart)
                AddToMap();
        }
    }
}
