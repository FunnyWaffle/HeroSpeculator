using Assets.Scripts.Heroes.Control;
using Assets.Scripts.StatModificators.Buffs;
using System;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [Serializable]
    public class DefenceAbility : Ability<HeroController>
    {
        [SerializeField] private float _reduceDamagePower;
        [SerializeField] private float _buffDuration;

        public float BuffDuration => _buffDuration;

        protected override void Execute(HeroController target)
        {
            target.ApplyBuff(
                new DefenceReductionBuff(_reduceDamagePower, _buffDuration));

        }
    }
}
