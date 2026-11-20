using UnityEngine;

namespace Assets.Scripts.Heroes.Control
{
    public class HeroRotator : MonoBehaviour
    {
        [SerializeField] private Transform _modelRig;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _rotationDistance = 2f;
        [SerializeField] private Vector3 _rotationOffset;

        public void Rotate(Transform target, Vector3 movementDirection, float deltaTime)
        {
            Vector3 direction = movementDirection;
            var offset = Quaternion.identity;

            if (target != null)
            {
                var targetDirection = target.position - _modelRig.position;

                if (targetDirection.sqrMagnitude
                    <= _rotationDistance * _rotationDistance)
                {
                    direction = targetDirection;
                    offset = Quaternion.Euler(_rotationOffset);
                }
            }

            direction.y = 0;

            if (direction.sqrMagnitude < 0.001f)
                return;

            var rotation = Quaternion.LookRotation(direction) * offset;

            _modelRig.rotation = Quaternion.Slerp(
                _modelRig.rotation,
                rotation,
                _rotationSpeed * deltaTime
            );
        }
    }
}
