using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerGroundLocomotionState : PlayerState
    {
        [SerializeField] private PlayerMoveAndRotateAction _moveAndRotate;

        public sealed override void StateUpdate()
        {
            if (!Character.Grounded)
                StateMachine.ChangeState<PlayerFallState>();
            if (PlayerInput.InputMap.Gameplay.Jump.triggered)
                StateMachine.ChangeState<PlayerJumpState>();
            if (PlayerInput.InputMap.Gameplay.Roll.triggered)
                StateMachine.ChangeState<PlayerRollState>();
            if (PlayerInput.InputMap.Gameplay.SwordAttack.triggered)
                StateMachine.ChangeState<PlayerFightState>();
            if (PlayerInput.InputMap.Gameplay.Parry.triggered)
                StateMachine.ChangeState<PlayerParryState>();
            OnGroundStateUpdated();
        }

        public sealed override void StateFixedUpdate()
        {
            _moveAndRotate.MoveAndRotate(Character, PlayerInput);
            OnGroundStateFixedUpdated();
        }

        protected virtual void OnGroundStateUpdated() { }
        protected virtual void OnGroundStateFixedUpdated() { }
    }
}
