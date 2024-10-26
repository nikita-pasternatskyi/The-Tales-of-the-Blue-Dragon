using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        public UnityEvent Entered;
        public UnityEvent Exited;

        protected StateMachine StateMachine { get; private set; }
        public void Initialize(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
            OnInit(stateMachine);
        }

        protected virtual void OnInit(StateMachine stateMachine) { }

        public virtual bool CanEnter() => true;

        public void Enter()
        {
            Entered?.Invoke();
            OnEnter();
        }

        public void Exit()
        {
            Exited?.Invoke();
            OnExit();
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void StateUpdate() { }
        public virtual void StateFixedUpdate() { }
    }

}