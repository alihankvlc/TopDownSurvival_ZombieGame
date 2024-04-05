using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DeadNation
{
    public interface InputHandler
    {
        public Vector2 Move { get; }
        public bool Attack { get; }
        public bool Inventory { get; }
    }

    public class InputManager : Singleton<InputManager>, InputHandler
    {
        [FormerlySerializedAs("m_PlayerInput")] [SerializeField] private PlayerInput _playerInput;

        private InputActionMap _inputActionMap;
        private InputAction _moveInputAction;
        private InputAction _attackInputAction;
        private InputAction _inventoryInputAction;

        public Vector2 Move { get; private set; }
        public bool Attack { get; private set; }
        public bool Inventory { get; private set; }

        private void Awake()
        {
            _inputActionMap = _playerInput.currentActionMap;

            _moveInputAction = _inputActionMap.FindAction("Move");
            _attackInputAction = _inputActionMap.FindAction("Attack");
            _inventoryInputAction = _inputActionMap.FindAction("Inventory");
        }
        private void OnEnable()
        {
            _inputActionMap.Enable();

            _moveInputAction.performed += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            _moveInputAction.canceled += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();

            _attackInputAction.performed +=
                (InputAction.CallbackContext context) => Attack = context.ReadValueAsButton();
            _attackInputAction.canceled +=
                (InputAction.CallbackContext context) => Attack = context.ReadValueAsButton();
        }

        private void Update()
        {
            Inventory = _inventoryInputAction.WasPressedThisFrame();
        }

        private void OnDisable()
        {
            _inputActionMap.Disable();

            _moveInputAction.performed -= (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            _moveInputAction.canceled -= (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            
            _attackInputAction.performed -=
                (InputAction.CallbackContext context) => Attack = context.ReadValueAsButton();
            _attackInputAction.canceled -=
                (InputAction.CallbackContext context) => Attack = context.ReadValueAsButton();
        }
    }
}