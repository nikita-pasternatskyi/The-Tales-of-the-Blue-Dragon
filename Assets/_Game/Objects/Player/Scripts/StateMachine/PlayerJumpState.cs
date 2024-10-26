using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerJumpState : PlayerState
    {
        [SerializeField] private float _jumpHeight;
        [SerializeField] private PlayerMoveAndRotateAction _moveAndRotate;

        public override void OnEnter()
        {
            Character.Jump(_jumpHeight);
        }

        public override void StateFixedUpdate()
        {
            _moveAndRotate.MoveAndRotate(Character, PlayerInput);
        }

        public override void StateUpdate()
        {
            if (Character.Grounded)
                StateMachine.ChangeState<PlayerIdleState>();
            if (Character.Velocity.y < 0)
                StateMachine.ChangeState<PlayerFallState>();
        }
    }
}
