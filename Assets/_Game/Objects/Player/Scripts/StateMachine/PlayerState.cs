using MP.Game.Objects.StateMachine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerState : State
    {
        protected Character.Character Character { get; private set; }
        protected PlayerInput PlayerInput { get; private set; }

        protected sealed override void OnInit(MP.Game.Objects.StateMachine.StateMachine stateMachine)
        {
            Character = stateMachine.GetComponent<Character.Character>();
            PlayerInput = stateMachine.GetComponent<PlayerInput>();
            StateInited();
        }

        protected virtual void StateInited() { }
    }
}
