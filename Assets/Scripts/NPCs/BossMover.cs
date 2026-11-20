using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.NPCs
{
    public class BossMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        private int _targetPriority;
        private IMoveTarget _target;
        private Vector3 _targetLastPosition;

        public float Radius => _agent.radius;

        private void Update()
        {
            if (_target == null)
                return;

            var position = _target.Position;

            if (Vector3.Distance(
                _targetLastPosition, position) > 0.1f)
            {
                SetDestination(position);
            }
        }

        public bool TrySetTarget(
            IMoveTarget moveTarget,
            int priority)
        {
            if (priority < _targetPriority
                || priority < 0)
                return false;

            _targetPriority = priority;
            _target = moveTarget;
            SetDestination(_target.Position);

            return true;
        }

        public void SetRadius(float units)
        {
            _agent.radius = units;
        }

        public void Stop()
        {
            _target = null;
            _targetPriority = 0;

            _agent.isStopped = true;
            _agent.ResetPath();
        }

        private void SetDestination(Vector3 position)
        {
            _targetLastPosition = position;

            var direction = Vector3.Normalize(
                transform.position - position);

            _agent.SetDestination(
                position
                + direction
                * (_target.GetBoundaryDistance(direction)
                + _agent.radius + 0.2f));
        }
    }

    public interface IMoveTarget
    {
        public Vector3 Position { get; }
        public float GetBoundaryDistance(Vector3 direction);
    }
}
