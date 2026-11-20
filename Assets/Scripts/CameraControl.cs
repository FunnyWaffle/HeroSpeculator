using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private Camera _camera;

        [SerializeField] private float _sensetivity = 1f;

        private bool _isGroundClicked;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            _input.Click.started += OnPlayerClickStarted;
            _input.Click.canceled += OnPlayerClickCanceled;
            _input.PointerDelta.performed += OnPointerMoved;
        }

        private void OnDisable()
        {
            _input.Click.started -= OnPlayerClickStarted;
            _input.Click.canceled -= OnPlayerClickCanceled;
            _input.PointerDelta.performed -= OnPointerMoved;
        }

        private void OnPlayerClickStarted(InputAction.CallbackContext context)
        {
            var pointerPosition = _input.MousePosition.ReadValue<Vector2>();

            var ray = _camera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out var _, float.MaxValue,
                LayerMask.GetMask("PlayerInteraction"),
                QueryTriggerInteraction.Collide))
                return;

            _isGroundClicked = true;
        }

        private void OnPointerMoved(InputAction.CallbackContext context)
        {
            if (!_isGroundClicked)
                return;

            var delta = context.ReadValue<Vector2>();
            var scaledDelta = delta * _sensetivity;

            var yawRotation = Quaternion.Euler(
                0f,
                transform.eulerAngles.y,
                0f);

            var localDisplacement = new Vector3(
                scaledDelta.x,
                0f,
                scaledDelta.y);

            var worldDisplacement = yawRotation * localDisplacement;

            transform.position += -worldDisplacement;
        }

        private void OnPlayerClickCanceled(InputAction.CallbackContext context)
        {
            _isGroundClicked = false;
        }
    }
}
