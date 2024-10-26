using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerRollState : PlayerState
    {
        [SerializeField] private float _rollCooldownTime;
        [SerializeField] private float _rollTime;
        [SerializeField] private Vector2 _jumpForce;

        private float _rollTimer;

        private float _lastSuccessfullEnterTime;

        public override bool CanEnter()
        {
            bool result = false;
            if (_rollTimer == 0)
                result = true;

            if (Time.realtimeSinceStartup - _lastSuccessfullEnterTime >= _rollCooldownTime + _rollTime)
                result = true;

            if (result)
                _lastSuccessfullEnterTime = Time.realtimeSinceStartup;

            return result;
        }

        public override void OnEnter()
        {
            Character.AlignToVector(PlayerInput.RelativeMovementInput, 0);
            Character.Jump(_jumpForce.y, _jumpForce.x, Character.transform.forward);
            _rollTimer = 0;
        }

        public override void StateUpdate()
        {
            _rollTimer += Time.deltaTime;
            if (_rollTimer >= _rollTime)
            {
                if (PlayerInput.InputMap.Gameplay.SwordAttack.IsPressed())
                {
                    StateMachine.ChangeState<PlayerFightState>();
                    return;
                }
                if (PlayerInput.AbsoluteMovementInput != Vector2.zero)
                {
                    if (PlayerInput.InputMap.Gameplay.Run.IsPressed())
                    {
                        StateMachine.ChangeState<PlayerRunState>();
                        return;
                    }
                    StateMachine.ChangeState<PlayerWalkState>();
                    return;
                }
                Character.Velocity = new Vector3(0, Character.Velocity.y, 0);
                StateMachine.ChangeState<PlayerIdleState>();
            }
        }

        public override void OnExit()
        {
        }
    }
}
