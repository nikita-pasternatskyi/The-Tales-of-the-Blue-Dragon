using UnityEngine;

namespace MP.Game.Objects.Environment
{
    public class SwitchToEnvironmentOnStart : MonoBehaviour
    {
        [SerializeField] private EnvironmentSettings _newEnvironment;
        [SerializeField] private float _transitionDuration;
        private void Start()
        {
            Environment.Instance.TransitionTo(_newEnvironment, _transitionDuration);
        }
    }
}
