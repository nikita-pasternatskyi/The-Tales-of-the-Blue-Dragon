using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerIdleState : PlayerGroundLocomotionState
    {
        protected override void OnGroundStateUpdated()
        {
            if (PlayerInput.AbsoluteMovementInput != Vector2.zero)
            {
                if (PlayerInput.InputMap.Gameplay.Run.triggered)
                {
                    StateMachine.ChangeState<PlayerRunState>();
                    return;
                }
                StateMachine.ChangeState<PlayerWalkState>();
            }
        }
    }
}
