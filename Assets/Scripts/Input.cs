using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class Input : MonoBehaviour
    {
        private InputSystem_Actions _actions;

        private void Awake()
        {
            _actions = new();
            _actions.Enable();
        }

        public InputAction Click => _actions.Player.Attack;
        public InputAction MousePosition => _actions.Player.MousePosition;
        public InputAction PointerDelta => _actions.Player.Look;
    }
}
