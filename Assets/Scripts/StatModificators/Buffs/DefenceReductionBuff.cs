using Assets.Scripts.Timers;
using System;
using UnityEngine;

namespace Assets.Scripts.StatModificators.Buffs
{
    public class DefenceReductionBuff
    {
        private readonly float _damageReduction;
        private readonly Timer _existingTime;

        public DefenceReductionBuff(float reduceDamageValue, float duration)
        {
            _damageReduction = reduceDamageValue;

            _existingTime = new Timer(duration);
            _existingTime.Trigger();

            _existingTime.Expired += TimerExpired;
        }

        public event Action Expired;

        public float Apply(float damage)
        {
            if (_existingTime.IsReady)
                return damage;

            Debug.Log($"Damage reduced by {_damageReduction}");

            return Mathf.Max(0f, damage - _damageReduction);
        }

        public void UpdateCooldown(float deltaTime)
        {
            _existingTime.Tick(deltaTime);
        }

        private void TimerExpired()
        {
            _existingTime.Expired -= TimerExpired;
            Expired?.Invoke();
        }
    }
}
