using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace tetris
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private TetrisController tetrisController;
        [SerializeField] private float movementDeadzone = 0.5f;
        [SerializeField] private float movementCooldown = 0.2f;
        [SerializeField] private float rotationDeadzone = 0.5f;
        [SerializeField] private float rotationCooldown = 0.25f;

        private TetrisSystem _tetrisSystem;
        private InputSystem _inputSystem;

        private float _timeAccumulator = 0;
        private Vector2Int _movementDirection;
        private float _timeToMove = 0;
        private int _rotationDirection;
        private float _timeToRotate = 0;


        private void Awake()
        {
            _inputSystem = new InputSystem();
        }

        private void Start()
        {
            _tetrisSystem = tetrisController.GetTetrisSystem();
        }


        private void OnEnable()
        {
            _inputSystem.Enable();

            _inputSystem.Tetris.Drop.performed += Drop;
            _inputSystem.Tetris.Undo.performed += Undo;
            _inputSystem.Tetris.Swap.performed += Swap;
            _inputSystem.Tetris.Pause.performed += Pause;
        }

        private void OnDisable()
        {
            _inputSystem.Disable();

            _inputSystem.Tetris.Drop.performed -= Drop;
            _inputSystem.Tetris.Undo.performed -= Undo;
            _inputSystem.Tetris.Swap.performed -= Swap;
            _inputSystem.Tetris.Pause.performed -= Pause;
        }

        private void Update()
        {
            if (tetrisController.IsPaused)
            {
                return;
            }

            _timeAccumulator += Time.deltaTime;

            MovementInput();
            RotationInput();

            if (_timeAccumulator >= _timeToMove && _movementDirection.magnitude > movementDeadzone)
            {
                _timeToMove = _timeAccumulator + movementCooldown;
                _tetrisSystem.Move(_movementDirection);
            }

            if (_timeAccumulator >= _timeToRotate && Mathf.Abs(_rotationDirection) > rotationDeadzone)
            {
                _timeToRotate = _timeAccumulator + rotationCooldown;
                _tetrisSystem.Rotate(_rotationDirection);
            }
        }

        private void MovementInput()
        {
            var input = _inputSystem.Tetris.Movement.ReadValue<Vector2>();
            if (input.magnitude < movementDeadzone)
            {
                _movementDirection = Vector2Int.zero;
                return;
            }

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                _movementDirection = input.x > 0 ? Vector2Int.right : Vector2Int.left;
            }
            else
            {
                _movementDirection = input.y > 0 ? Vector2Int.zero : Vector2Int.down;
            }
        }

        private void RotationInput()
        {
            var input = _inputSystem.Tetris.Rotate.ReadValue<float>();
            if (Mathf.Abs(input) < rotationDeadzone)
            {
                _rotationDirection = 0;
                return;
            }

            _rotationDirection = (int)Mathf.Sign(input);
        }

        private void Drop(InputAction.CallbackContext context)
        {
            _tetrisSystem.QuickDrop();
        }

        private void Undo(InputAction.CallbackContext context)
        {
            tetrisController.Undo();
        }

        private void Swap(InputAction.CallbackContext context)
        {
            _tetrisSystem.Swap();
        }

        private void Pause(InputAction.CallbackContext context)
        {
            tetrisController.IsPaused = !tetrisController.IsPaused;
        }
    }
}