using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts
{
    [System.Serializable]
    public struct PlayerAttack
    {
        public float Radius;
        public float Distance;
        public float Damage;
        public float Cooldown;
        public float HitDelay;
        public float ReturnDelay;
        public Vector2 JumpForce;
        public UnityEvent Triggered;
    }
}
