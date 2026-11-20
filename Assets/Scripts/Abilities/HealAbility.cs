using Assets.Scripts.Heroes.Control;
using System;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [Serializable]
    public class HealAbility : Ability<HeroController>
    {
        [SerializeField] private float _healPower = 10f;

        protected override void Execute(HeroController target)
        {
            target.TakeHeal(_healPower);
        }
    }
}
