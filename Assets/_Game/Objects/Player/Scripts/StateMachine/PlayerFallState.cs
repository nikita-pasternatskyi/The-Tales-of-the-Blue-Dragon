using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerFallState : PlayerState
    {
        [SerializeField] private PlayerMoveAndRotateAction _moveAndRotate;

        public override void StateFixedUpdate()
        {
            _moveAndRotate.MoveAndRotate(Character, PlayerInput);
        }

        public override void StateUpdate()
        {
            if (Character.Grounded)
                StateMachine.ChangeState<PlayerIdleState>();
        }
    }
}
