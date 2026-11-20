using Assets.Scripts.Abilities;
using Assets.Scripts.Heroes.Control;
using UnityEngine;

namespace Assets.Scripts.Heroes.HeroSpecializations
{
    public class Heal : HeroSpecialization
    {
        [SerializeField] private HealAbility _ability;
        [SerializeField] private HeroController[] _allies;
        [SerializeField] private Animator _animator;

        private HeroController _target;

        private readonly int _healTriggerHash = Animator.StringToHash("Heal Triggered");

        protected override Ability Ability => _ability;
        protected override Transform Target
        {
            get
            {
                return _target == null
                    ? null
                    : _target.transform;
            }
        }

        public override void Use()
        {
            var allieWothLowestHP = FindAllieWithLowestHP();

            _target = allieWothLowestHP;

            if (allieWothLowestHP == null)
                return;

            if (_ability.TryUse(allieWothLowestHP))
                _animator.SetTrigger(_healTriggerHash);
        }

        private HeroController FindAllieWithLowestHP()
        {
            HeroController hero = null;
            var lowestHP = float.MaxValue;

            foreach (var allie in _allies)
            {
                var hp = allie.HealthPoints;

                if (hp <= 0
                    || allie.IsHpFull)
                    continue;

                if (hp < lowestHP)
                {
                    lowestHP = hp;
                    hero = allie;
                }
            }

            return hero;
        }

        protected override void UpdateAnimations()
        {
        }
    }
}