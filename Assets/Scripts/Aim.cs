using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    [Serializable]
    public class Aim
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _error;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public Vector3 GetPoint()
        {
            var position = _target.position;

            return new Vector3(
                Random.Range(position.x - _error,
                        position.x + _error),
                Random.Range(position.y - _error,
                        position.y + _error),
                position.z);
        }
    }
}
