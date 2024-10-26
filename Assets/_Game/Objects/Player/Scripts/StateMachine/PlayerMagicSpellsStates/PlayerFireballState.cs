using System.Collections.Generic;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine.PlayerMagicSpellsStates
{
    public class PlayerMagicAbilities : MonoBehaviour
    {
        [SerializeField] private List<PlayerMagicState> _magicAttacks;
    }

    public class PlayerMagicState : PlayerState
    {
        [field: SerializeField] public float SpellManaCost { get; private set; }
        [field: SerializeField] public float SpellCooldown { get; private set; }
        [field: SerializeField] public float SpellDuration { get; private set; }

        protected float LifeTime { get; private set; }

        public override void StateUpdate()
        {
            LifeTime += Time.deltaTime;
            if (LifeTime >= SpellDuration)
            {
                StateMachine.ChangeState<PlayerIdleState>();
                LifeTime = 0;
                return;
            }
        }
    }

    public class PlayerFireballState : PlayerMagicState
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _spawnPosition;

        public override void OnEnter()
        {
            Instantiate(_projectilePrefab, _spawnPosition.position, _spawnPosition.rotation, null);
        }
    }
}
