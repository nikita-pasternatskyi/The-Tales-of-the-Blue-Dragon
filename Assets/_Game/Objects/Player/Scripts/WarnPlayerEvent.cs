using MP.Game.Objects.Enemy.Scripts;
using MP.Game.Utils;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    [CreateAssetMenu(menuName = "MP/Events/Warn Player")]
    public class PlayerDangerEvent : GenericScriptableEvent<GameObject, EnemyAttack>
    {
    }
}
