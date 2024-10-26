using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

namespace MP.Game.Objects.FinalBoss
{
    public class FinalBossJumpState : FinalBossState
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpYOffset;
        private bool _runningCoroutine;

        public override void OnEnter()
        {
            if(Enemy.Player == null)
            {
                Enemy.FoundPlayer(FindObjectOfType<Player.Scripts.Player>());
            }
            var targetPos = Enemy.Player.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }

        public void Jump()
        {
            if (!_runningCoroutine)
                StartCoroutine(JumpToTarget());
        }

        //source https://discussions.unity.com/t/3d-how-to-make-player-jump-to-a-target-rather-than-towards-it/848848/10
        private IEnumerator JumpToTarget()
        {
            Debug.Log("Jump");
            _runningCoroutine = true;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = Enemy.Player.transform.position;
            targetPosition.y = transform.position.y;
            Vector3 handlePosition = Vector3.Lerp(startPosition, targetPosition, 0.5f);
            handlePosition.y += _jumpHeight;

            float distance = (startPosition - targetPosition).magnitude;
            float duration = distance / _movementSpeed;

            for (float f = 0; f < 1; f += Time.deltaTime / duration)
            {
                transform.position = Vector3.Lerp(
                    Vector3.Lerp(
                        startPosition,
                        handlePosition,
                        f),
                    Vector3.Lerp(
                        handlePosition,
                        targetPosition,
                        f),
                    f);

                yield return null;
            }
            transform.position = targetPosition;
            Land();
            _runningCoroutine = false;
        }

        public void Land()
        {
            StateMachine.ChangeState<FinalBossMonsterIdle>();
        }

    }
}
