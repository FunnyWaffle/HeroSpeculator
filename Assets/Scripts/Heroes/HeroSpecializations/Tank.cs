using Assets.Scripts.Abilities;
using Assets.Scripts.Heroes.Control;
using UnityEngine;

namespace Assets.Scripts.Heroes.HeroSpecializations
{
    public class Tank : HeroSpecialization
    {
        [SerializeField] private DefenceAbility _defence;
        [SerializeField] private Animator _animator;
        [SerializeField] private Boss _target;
        [SerializeField] private HeroController _selfController;

        protected override Ability Ability => _defence;
        protected override Transform Target => _target.transform;

        public override void Use()
        {
            if (_target.IsDead)
                return;

            _defence.TryUse(_selfController);
        }

        protected override void UpdateAnimations()
        {
        }
    }
}
