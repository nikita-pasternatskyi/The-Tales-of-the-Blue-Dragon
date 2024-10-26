namespace MP.Game.Objects.Player.Scripts.StateMachine.PlayerHorseStates
{
    public class PlayerHorseState : PlayerState
    {
        public sealed override void StateUpdate()
        {
            if (PlayerInput.InputMap.Gameplay.Jump.triggered)
            {
                StateMachine.ChangeState<PlayerIdleState>();
            }
            OnStateUpdated();
        }
        protected virtual void OnStateUpdated() { }
    }
}
