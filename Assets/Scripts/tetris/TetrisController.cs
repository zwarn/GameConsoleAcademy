﻿using System;
using System.Collections.Generic;
using System.Linq;
using tetris.score;
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
        private bool isFinished = false;

        private Stack<TetrisState> _history = new Stack<TetrisState>();
        private TetrisState _currentState = null;

        private void Awake()
        {
            var tetrisState = new TetrisState(width, height, pieceGenerator.GeneratePieces(tileAmount).ToList(), null,
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
            if (isFinished)
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

        public void Undo()
        {
            if (!isFinished && _history.Count > 0)
            {
                var tetrisState = _history.Count == 1 ? _history.Peek() : _history.Pop();
                _tetrisSystem.LoadState(tetrisState);
                _currentState = tetrisState;
            }
        }

        private void GameFinished()
        {
            isFinished = true;
            int score = TetrisScore.Score(_tetrisSystem.Tiles(), width, height);
            Debug.Log($"score : {score}");
        }

        public TetrisSystem GetTetrisSystem()
        {
            return _tetrisSystem;
        }
    }
}