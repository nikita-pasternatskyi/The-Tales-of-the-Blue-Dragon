using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private GameObject[] _statesGameObjects;
        [SerializeField] private State _initialState;
        [SerializeField] private bool _debug;

        public UnityEvent<State> StateChanged;
        public State CurrentState { get; private set; }
        private Dictionary<Type, State> _statesDictionary;

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (!_debug)
                return;
            GUI.skin.label.fontSize = 48;
            GUI.Label(new Rect(0, 0,
                Screen.currentResolution.width * 0.25f,
                Screen.currentResolution.height * 0.25f),
                CurrentState.ToString().Split('.').Last());
        }
#endif

        private void Awake()
        {
            InitiateStates();
            ChangeState(_initialState);
        }

        private void Update()
        {
            CurrentState.StateUpdate();
        }

        private void FixedUpdate()
        {
            CurrentState.StateFixedUpdate();
        }

        public void ChangeState(State newState)
        {
            if (newState == CurrentState || !newState.CanEnter())
                return;

            if (CurrentState)
            {
                CurrentState.Exit();
            }
            CurrentState = newState;
            CurrentState.Enter();
            StateChanged?.Invoke(CurrentState);
        }

        public void ChangeState<T>() where T : State
        {
            if (_statesDictionary.TryGetValue(typeof(T), out State state))
            {
                ChangeState(state);
                return;
            }
            Debug.LogError($"No state of type {typeof(T)} is found on the object", gameObject);
        }

        public T GetState<T>() where T : State
        {
            InitiateStates();

            if (_statesDictionary.TryGetValue(typeof(T), out State state))
            {
                return state as T;
            }
            Debug.LogError($"No state of type {typeof(T)} is found on the object", gameObject);
            return null;
        }

        private void InitiateStates()
        {
            if (_statesDictionary != null)
                return;
            _statesDictionary = new Dictionary<Type, State>();
            foreach (var state in transform.GetComponents<State>())
            {
                state.Initialize(this);
                if(!_statesDictionary.ContainsKey(state.GetType()))
                    _statesDictionary.Add(state.GetType(), state);
            }
            foreach (var gameObject in _statesGameObjects)
            {
                foreach (var state in gameObject.GetComponents<State>())
                {
                    state.Initialize(this); 
                    if (!_statesDictionary.ContainsKey(state.GetType()))
                        _statesDictionary.Add(state.GetType(), state);
                }
            }
        }
    }

}