using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine.PlayerHorseStates
{
    public class PlayerHorseIdleState : PlayerHorseState
    {
        public override void OnEnter()
        {
        }

        protected override void OnStateUpdated()
        {
            if (PlayerInput.AbsoluteMovementInput != Vector2.zero)
            {
                if (PlayerInput.InputMap.Gameplay.Run.triggered)
                {
                    StateMachine.ChangeState<PlayerHorseRunState>();
                    return;
                }
                StateMachine.ChangeState<PlayerHorseWalkState>();
            }
        }
    }
}
