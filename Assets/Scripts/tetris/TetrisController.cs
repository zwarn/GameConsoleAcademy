using System;
using System.Collections.Generic;
using System.Linq;
using tetris.score;
using UnityEngine;
using Zenject;

namespace tetris
{
    public class TetrisController : MonoBehaviour
    {
        [SerializeField] public int width = 10;
        [SerializeField] public int height = 20;
        [SerializeField] private float gameplaySpeed = 1f;
        [SerializeField] private int tileAmount = 25;
        
        [Inject] private PieceGenerator _pieceGenerator;

        public bool IsPaused { get; set; }

        private TetrisSystem _tetrisSystem;
        private float _timeAccumulator;
        private bool _isFinished = false;

        private readonly Stack<TetrisState> _history = new Stack<TetrisState>();
        private TetrisState _currentState = null;

        private void Awake()
        {
            var tetrisState = new TetrisState(width, height, _pieceGenerator.GeneratePieces(tileAmount).ToList(), null,
                new Dictionary<Vector2Int, Tile>());
            _tetrisSystem = new TetrisSystem(tetrisState);
            RecordHistory(tetrisState);
        }

        private void Start()
        {
            _tetrisSystem.OnGameFinish += GameFinished;
            _tetrisSystem.OnPiecePlaced += RecordHistory;
        }

        private void OnDestroy()
        {
            _tetrisSystem.OnGameFinish -= GameFinished;
            _tetrisSystem.OnPiecePlaced -= RecordHistory;
        }

        private void Update()
        {
            if (_isFinished || IsPaused)
            {
                return;
            }

            _timeAccumulator += Time.deltaTime;
            if (_timeAccumulator >= gameplaySpeed)
            {
                _timeAccumulator -= gameplaySpeed;
                _tetrisSystem.Drop();
            }
        }

        private void RecordHistory(Piece _)
        {
            var tetrisState = _tetrisSystem.GetState();
            if (_currentState != null)
            {
                RecordHistory(_currentState);
            }

            _currentState = tetrisState;
        }

        private void RecordHistory(TetrisState tetrisState)
        {
            _history.Push(tetrisState);
        }

        private void GameFinished()
        {
            _isFinished = true;
            int score = TetrisScore.Score(_tetrisSystem.Tiles(), width, height);
            Debug.Log($"score : {score}");
        }

        public TetrisSystem GetTetrisSystem()
        {
            return _tetrisSystem;
        }

        public void PerformMove(Vector2Int direction)
        {
            if (_isFinished)
            {
                return;
            }

            _tetrisSystem.Move(direction);
        }

        public void PerformRotate(int direction)
        {
            if (_isFinished)
            {
                return;
            }

            _tetrisSystem.Rotate(direction);
        }

        public void PerformQuickDrop()
        {
            if (_isFinished)
            {
                return;
            }

            _tetrisSystem.QuickDrop();
        }

        public void PerformUndo()
        {
            if (!_isFinished && _history.Count > 0)
            {
                var tetrisState = _history.Count == 1 ? _history.Peek() : _history.Pop();
                _tetrisSystem.LoadState(tetrisState);
                _currentState = tetrisState;
            }
        }

        public void PerformSwap()
        {
            if (_isFinished)
            {
                return;
            }

            _tetrisSystem.Swap();
        }
    }
}