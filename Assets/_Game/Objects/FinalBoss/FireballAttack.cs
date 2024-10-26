using UnityEngine;

namespace MP.Game.Objects.FinalBoss
{
    public class FireballAttack : MonoBehaviour
    {
        [SerializeField] private float _scaleTime;
        [SerializeField] private float _flySpeed;
        [SerializeField] private float _lifeTime;
        [SerializeField] private AnimationCurve _scaleCurve;
        private float _lifeTimer;
        private float _scaleTimer;

        private void OnEnable()
        {
            _scaleTimer = 0;
            _lifeTimer = 0;
        }

        private void Update()
        {
            if(_scaleTimer <= _scaleTime)
            {
                _scaleTimer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _scaleCurve.Evaluate(_scaleTimer / _scaleTime));
            }
            if(_scaleTimer >= _scaleTime)
            {
                if (_lifeTimer <= _lifeTime)
                {
                    _lifeTimer += Time.deltaTime;
                    transform.position += transform.forward * _flySpeed * Time.deltaTime;
                }
                if(_lifeTimer >= _lifeTime)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
