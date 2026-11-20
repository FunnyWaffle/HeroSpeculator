using Assets.Scripts.Heroes.Control;
using System;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    [Serializable]
    public class BossMeleeAttack : BossCombatMove
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackRange = 2.5f;

        private Transform _transform;

        private float _attackRangeSqr;

        public void Initialize(
            Animator animator,
            Transform transform,
            BossTargetFinder targetFinder)
        {
            Initialize(animator, targetFinder);

            _transform = transform;

            _attackRangeSqr = _attackRange * _attackRange;
        }

        protected override void OnPerformed(HeroController target)
        {
            if (TryAttack(target))
                return;

            if (!TryGetCurrentTarget(out target))
                return;

            TryAttack(target);
        }

        protected override bool OnCanPerform()
        {
            return IsReachable(GetSavedTarget());
        }

        private bool TryAttack(HeroController target)
        {
            if (!IsReachable(target))
                return false;

            target.TakeDamage(_damage);
            return true;
        }

        private bool IsReachable(HeroController target)
        {
            var axis = target.Position -
                       _transform.position;

            var sqrDistance = Vector3.SqrMagnitude(axis);

            if (sqrDistance > _attackRangeSqr)
                return false;

            return true;
        }
    }
}
