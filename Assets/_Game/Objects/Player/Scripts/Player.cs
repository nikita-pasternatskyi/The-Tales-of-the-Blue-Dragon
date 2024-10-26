using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    [RequireComponent(typeof(Character.Character))]
    [RequireComponent(typeof(Health.Health))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerInteraction))]
    [RequireComponent(typeof(Objects.StateMachine.StateMachine))]
    public class Player : MonoBehaviour
    {
    }
}
