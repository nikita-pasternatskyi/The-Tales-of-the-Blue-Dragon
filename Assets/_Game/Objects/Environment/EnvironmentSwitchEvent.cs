using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Environment
{
    [CreateAssetMenu(menuName = "MP/Environment/EnvironmentSwitchEvent")]
    public class EnvironmentSwitchEvent : GenericScriptableEvent<EnvironmentSettings, float>
    {
        [SerializeField] private EnvironmentSettings _targetSettings;
        [SerializeField] private float _transitionDuration;

        public override void Invoke(EnvironmentSettings obj1, float obj2)
        {
            Environment.Instance.TransitionTo(obj1, obj2);
        }

        public override void Invoke()
        {
            base.Invoke();
            Environment.Instance.TransitionTo(_targetSettings, _transitionDuration);
        }
    }
}
