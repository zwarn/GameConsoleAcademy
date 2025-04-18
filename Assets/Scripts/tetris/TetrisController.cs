using System;
using UnityEngine;

namespace tetris
{
    public class TetrisController : MonoBehaviour
    {

        [SerializeField]
        public int width = 10;
        [SerializeField]
        public int height = 20;
        [SerializeField]
        private float gameplaySpeed = 1f;
        
        private TetrisSystem _tetrisSystem;
        private float _timeAccumulator;

        private void Awake()
        {
            _tetrisSystem = new TetrisSystem(width, height);
        }

        private void Update()
        {
            _timeAccumulator += Time.deltaTime;
            if (_timeAccumulator >= gameplaySpeed)
            {
                _timeAccumulator -= gameplaySpeed;
                _tetrisSystem.Drop();
            }
            
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _tetrisSystem.Move(Vector2Int.left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _tetrisSystem.Move(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _tetrisSystem.Move(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _tetrisSystem.Rotate(-1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _tetrisSystem.Rotate(1);
            }
        }

        public TetrisSystem GetTetrisSystem()
        {
            return _tetrisSystem;
        }
    }
}