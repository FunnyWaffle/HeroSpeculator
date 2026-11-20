using Assets.Scripts.Abilities;
using System;
using UnityEngine;

namespace Assets.Scripts.Heroes.HeroSpecializations
{
    [Serializable]
    public class DamageDealer : HeroSpecialization
    {
        [SerializeField] private DamageAbility _damage;
        [SerializeField] private Animator _animator;
        [SerializeField] private Boss _target;
        [SerializeField] private Aim _aim;

        private readonly int _hasTargetHash = Animator.StringToHash("Has Target");
        private readonly int _shotTriggerHash = Animator.StringToHash("Shot Triggered");
        private readonly int _drawArrowSpeedMultiplierHash = Animator.StringToHash("Draw Arrow Speed Multiplier");

        private float _drawArrowSpeedMultiplier;

        protected override Ability Ability => _damage;
        protected override Transform Target => _target.transform;

        private void Start()
        {
            _aim.SetTarget(Target);

            var drawArrowClipLength = Array.Find(_animator
                 .runtimeAnimatorController.animationClips
                 , c => c.name == "HumanF@BowShot01 - Load")
                 .length;

            _drawArrowSpeedMultiplier =
                drawArrowClipLength / _damage.Cooldown;

            _animator.SetFloat(
                _drawArrowSpeedMultiplierHash,
                _drawArrowSpeedMultiplier);
        }

        public override void Use()
        {
            if (_target.IsDead)
                return;

            if (_damage.TryUse(_aim.GetPoint()))
            {
                _animator.SetTrigger(_shotTriggerHash);
            }
        }

        protected override void UpdateAnimations()
        {
            if (_target.IsDead)
                return;

            _animator.SetBool(_hasTargetHash, _target != null);
        }
    }
}
