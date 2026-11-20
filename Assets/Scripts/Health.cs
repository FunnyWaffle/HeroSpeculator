using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Health
    {
        [SerializeField] private float _count = 100f;
        [SerializeField] private float _max = 100f;

        public float Current => _count;
        public float Max => _max;

        public void Initialize()
        {
            _count = Mathf.Clamp(_count, 0, _max);
        }

        public void Reduce(float amount)
        {
            _count -= amount;
            Validate();
        }

        public void Restore(float amount)
        {
            _count += amount;
            Validate();
        }

        public bool IsFull()
        {
            return _count == _max;
        }

        private void Validate()
        {
            _count = Mathf.Clamp(_count, 0, _max);
        }
    }
}
