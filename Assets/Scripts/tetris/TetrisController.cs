using System;
using UnityEngine;

namespace tetris
{
    public class TetrisController : MonoBehaviour
    {
        [SerializeField] public int width = 10;
        [SerializeField] public int height = 20;
        [SerializeField] private float gameplaySpeed = 1f;
        [SerializeField] private int tileAmount = 25;
        [SerializeField] private PieceGenerator pieceGenerator;

        private TetrisSystem _tetrisSystem;
        private float _timeAccumulator;

        private void Awake()
        {
            _tetrisSystem = new TetrisSystem(width, height, pieceGenerator.GeneratePieces(tileAmount));
        }

        private void Update()
        {
            if (_tetrisSystem.Finished)
            {
                return;
            }

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
                _tetrisSystem.Rotate(1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _tetrisSystem.Rotate(-1);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _tetrisSystem.Swap();
            }
        }

        public TetrisSystem GetTetrisSystem()
        {
            return _tetrisSystem;
        }
    }
}