using UnityEngine;

namespace MP.Game.Objects.FinalBoss
{

    public class FinalBossFireBreathState : FinalBossState
    {
        [SerializeField] private ParticleSystem _fireParticleSystem;

        public override void OnEnter()
        {
            var targetPos = Enemy.Player.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }

        public void StartFire()
        {
            _fireParticleSystem.gameObject.SetActive(true);
        }

        public void StopFire()
        {
            _fireParticleSystem.Stop();
        }
    }
}
