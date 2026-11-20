using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Heroes.Control
{
    [Serializable]
    public class HeroMover
    {
        [SerializeField] private Input _input;
        private Camera _camera;
        private NavMeshAgent _agent;
        private Transform _transform;

        private int _heroMask;
        private int _groundMask;

        private bool _clicked;

        public void Initialize(
            Camera camera,
            NavMeshAgent agent,
            Transform transform)
        {
            _camera = camera;
            _agent = agent;
            _transform = transform;

            _heroMask =
            LayerMask.GetMask("PlayerInteraction");
            _groundMask =
            LayerMask.GetMask("Ground");
        }

        public void Enable()
        {
            _input.Click.started += OnClickStarted;
            _input.Click.canceled += OnClickEnded;
        }

        public void Disable()
        {
            _input.Click.started -= OnClickStarted;
            _input.Click.canceled -= OnClickEnded;
        }

        private void OnClickStarted(InputAction.CallbackContext context)
        {
            var mousePosition = _input.MousePosition.ReadValue<Vector2>();

            var ray = _camera.ScreenPointToRay(mousePosition,
                Camera.MonoOrStereoscopicEye.Mono);

            if (Physics.Raycast(ray, out var hit,
                float.PositiveInfinity,
                _heroMask,
                QueryTriggerInteraction.Collide)
                && hit.transform == _transform)
                _clicked = true;
        }

        private void OnClickEnded(InputAction.CallbackContext context)
        {
            if (_clicked == false)
                return;

            _clicked = false;

            var mousePosition = _input.MousePosition.ReadValue<Vector2>();

            var ray = _camera.ScreenPointToRay(
                mousePosition,
                Camera.MonoOrStereoscopicEye.Mono);

            if (!Physics.Raycast(
                ray,
                out var hit,
                float.PositiveInfinity,
                _groundMask,
                QueryTriggerInteraction.Ignore))
                return;

            _agent.SetDestination(hit.point);
        }
    }
}
