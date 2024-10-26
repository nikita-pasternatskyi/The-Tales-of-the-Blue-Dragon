using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerWalkState : PlayerGroundLocomotionState
    {

        protected override void OnGroundStateUpdated()
        {
            if (PlayerInput.AbsoluteMovementInput == Vector2.zero)
            {
                StateMachine.ChangeState<PlayerIdleState>();
                return;
            }
            if (PlayerInput.InputMap.Gameplay.Run.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
            {
                StateMachine.ChangeState<PlayerRunState>();
                return;
            }
        }
    }
}
