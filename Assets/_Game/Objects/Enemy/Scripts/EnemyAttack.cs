using UnityEngine;

namespace MP.Game.Objects.Enemy.Scripts
{
    [System.Serializable]
    public struct EnemyAttack
    {
        public float Damage;
        public float AttackDistance;
        public float ExtraDodgeDistance;
        [Tooltip("The full duration of the attack")] public float AttackDurationTime;
        [Tooltip("Just how early will the player be able to parry?")] public float DangerWarningTime;

        public float DangerWarningActivationTime() => AttackDurationTime - DangerWarningTime;
    }
}
