using Assets.Scripts.NPCs;
using UnityEngine;

namespace Assets.Scripts
{
    public class BossCombatController : MonoBehaviour
    {
        [SerializeField] private BossTargetFinder _targetFinder;
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;
        [SerializeField] private BossMover _bossMover;
        [SerializeField] private BossMeleeAttack _meleeAttack;
        [SerializeField] private BossRockThrow _rockThrow;

        private Health _health;

        private CombatState _combatState;
        private bool _initialized;

        private void Awake()
        {
            _targetFinder.Initialize(transform);

            _meleeAttack.Initialize(
                _animator,
                transform,
                _targetFinder);

            _rockThrow.Initialize(
                _animator,
                transform,
                _bossMover,
                _collider,
                _targetFinder);
        }

        private void OnEnable()
        {
            _meleeAttack.Performed += OnMeleeAttackPerformed;
            _rockThrow.Performed += OnThrowPerformed;
        }

        private void OnDisable()
        {
            _meleeAttack.Performed -= OnMeleeAttackPerformed;
            _rockThrow.Performed -= OnThrowPerformed;
        }

        private void Update()
        {
            if (_health.Current <= 0)
                return;

            var deltaTime = Time.deltaTime;
            _meleeAttack.Update(deltaTime);
            _rockThrow.Update(deltaTime);
            _targetFinder.Invalidate();

            if (_combatState != CombatState.Idle)
                return;
            if (_targetFinder.TryGetCurrent(out var heroTarget))
                _bossMover.TrySetTarget(heroTarget.Hero, BossTargetPriorities.Hero);
            else
                return;

            if (_rockThrow.CanPerform())
            {
                _rockThrow.TryPerform(heroTarget);
                _combatState = CombatState.Throwing;

            }
            else if (_meleeAttack.CanPerform())
            {
                _meleeAttack.TryPerform(heroTarget);
                _combatState = CombatState.Attacking;
            }
        }

        public void Initialize(Health health)
        {
            if (_initialized)
                return;

            _health = health;

            _initialized = true;
        }

        private void OnThrowPerformed()
        {
            _combatState = CombatState.Idle;
        }

        private void OnMeleeAttackPerformed()
        {
            _combatState = CombatState.Idle;
        }
    }

    public enum CombatState
    {
        Idle,
        Throwing,
        Attacking
    }

    public static class BossTargetPriorities
    {
        public static readonly int Hero = 0;
        public static readonly int Rock = 1;
    }
}
