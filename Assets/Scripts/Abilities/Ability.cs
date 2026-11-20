using Assets.Scripts.Timers;
using System;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [Serializable]
    public abstract class Ability
    {
        [SerializeField] protected Timer _cooldown;

        public float Cooldown => _cooldown.Duration;

        public void UpdateCooldown(float deltaTime)
        {
            _cooldown.Tick(deltaTime);
        }
    }

    [Serializable]
    public abstract class Ability<T> : Ability
    {
        public bool TryUse(T targetContainer)
        {
            if (!_cooldown.IsReady)
                return false;

            Execute(targetContainer);

            _cooldown.Trigger();
            return true;
        }

        protected abstract void Execute(T targetContainer);
    }
}
