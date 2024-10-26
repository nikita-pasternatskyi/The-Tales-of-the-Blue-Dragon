using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Health
{
    [CreateAssetMenu(menuName ="MP/Events/Change Health Event")]
    public class ChangeHealthEvent : GenericScriptableEvent<float, HealthAction>
    {
    }
}
