using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace tetris
{
    public class TetrisSystem
    {
        public event Action<Piece> OnPiecePlaced;
        public event Action<Piece> OnPieceSpawned;
        public event Action<Vector2Int> OnMovePiece;
        public event Action<int> OnRotatePiece;
        public event Action<Piece> OnUpdateShadow;
        public event Action<Piece> OnUpdateSwap;
        public event Action<Dictionary<Vector2Int, Tile>> OnPlacedTilesChanged;
        public event Action OnGameFinish;

        private int _width;
        private int _height;
        private Dictionary<Vector2Int, Tile> _placedTiles = new();
        private Queue<Piece> _pieces;

        public Piece CurrentPiece;
        public Piece CurrentShadow;
        public Piece CurrentSwap;

        public TetrisSystem(TetrisState state)
        {
            _width = state.Width;
            _height = state.Height;
            _pieces = new Queue<Piece>(state.UpcomingPieces);

            CurrentSwap = state.Swap;
            if (CurrentSwap != null)
            {
                CurrentSwap.Position = Vector2Int.zero;
            }

            UpdateSwapEvent(CurrentSwap);

            _placedTiles = state.PlacedTiles.ToDictionary(pair => pair.Key, pair => pair.Value);
            PlacedTilesChanged(_placedTiles);

            DequeueNextPiece();
            UpdateShadow();
        }

        public TetrisState GetState()
        {
            return new TetrisState(_width, _height, _pieces.ToList(), CurrentSwap,
                _placedTiles.ToDictionary(pair => pair.Key, pair => pair.Value));
        }

        public void LoadState(TetrisState state)
        {
            _width = state.Width;
            _height = state.Height;
            _pieces = new Queue<Piece>(state.UpcomingPieces);

            CurrentSwap = state.Swap;
            if (CurrentSwap != null)
            {
                CurrentSwap.Position = Vector2Int.zero;
            }

            UpdateSwapEvent(CurrentSwap);

            _placedTiles = state.PlacedTiles.ToDictionary(pair => pair.Key, pair => pair.Value);
            PlacedTilesChanged(_placedTiles);

            DequeueNextPiece();
            UpdateShadow();
        }

        private void UpdateShadow()
        {
            if (CurrentPiece == null)
            {
                CurrentShadow = null;
                UpdateShadowEvent(CurrentShadow);
                return;
            }

            var shadow = CurrentPiece.Copy();
            while (!IsColliding(shadow))
            {
                shadow.Move(Vector2Int.down);
            }

            shadow.Move(Vector2Int.up);
            CurrentShadow = shadow;
            UpdateShadowEvent(CurrentShadow);
        }

        private bool IsInBounds(Vector2Int position)
        {
            return position.x >= 0 && position.x < _width && position.y >= 0;
        }

        private bool IsColliding(Piece piece)
        {
            return !piece.GetRotatedTranslatedTiles().Select(tile => tile.Key)
                .All(pos => IsInBounds(pos) && !_placedTiles.ContainsKey(pos));
        }

        public List<Piece> NextPieces(int num)
        {
            return num < 0 ? _pieces.ToList() : _pieces.Take(num).ToList();
        }

        public int RemainingPieces()
        {
            return _pieces.Count;
        }

        public void Move(Vector2Int direction)
        {
            if (CurrentPiece == null)
            {
                return;
            }

            if (CanMove(CurrentPiece, direction))
            {
                DoMove(direction);
            }
        }

        private bool CanMove(Piece piece, Vector2Int direction)
        {
            var copy = piece.Copy();
            copy.Move(direction);

            return !IsColliding(copy);
        }

        private void DoMove(Vector2Int direction)
        {
            CurrentPiece.Move(direction);
            MovePieceEvent(direction);
            UpdateShadow();
        }

        public void Rotate(int direction)
        {
            if (CurrentPiece == null)
            {
                return;
            }

            if (CanRotate(CurrentPiece, direction))
            {
                DoRotate(direction);
            }
        }

        private bool CanRotate(Piece piece, int direction)
        {
            var copy = piece.Copy();
            copy.Rotate(direction);

            return !IsColliding(copy);
        }

        private void DoRotate(int direction)
        {
            CurrentPiece.Rotate(direction);
            RotatePieceEvent(direction);
            UpdateShadow();
        }

        public void Swap()
        {
            if (CurrentPiece == null || _pieces.Count == 0)
            {
                return;
            }

            if (CurrentSwap == null)
            {
                CurrentSwap = CurrentPiece;
                DequeueNextPiece();
            }
            else
            {
                var swap = CurrentPiece;
                MakeCurrentPiece(CurrentSwap);
                CurrentSwap = swap;
            }

            CurrentSwap.Position = Vector2Int.zero;
            UpdateSwapEvent(CurrentSwap);
        }

        public void Drop()
        {
            if (CurrentPiece == null)
            {
                return;
            }

            if (CanMove(CurrentPiece, Vector2Int.down))
            {
                DoMove(Vector2Int.down);
                UpdateShadow();
            }
            else
            {
                LockPiece();
            }
        }

        public void QuickDrop()
        {
            if (CurrentPiece == null)
            {
                return;
            }

            while (CanMove(CurrentPiece, Vector2Int.down))
            {
                DoMove(Vector2Int.down);
            }

            LockPiece();
        }

        private void LockPiece()
        {
            foreach (var pair in CurrentPiece.GetRotatedTranslatedTiles())
            {
                _placedTiles[pair.Key] = pair.Value;
                if (pair.Key.y >= _height)
                {
                    FinishGameEvent();
                }
            }

            PiecePlacedEvent(CurrentPiece);
            DequeueNextPiece();
        }

        private void DequeueNextPiece()
        {
            if (_pieces.Count == 0)
            {
                MakeCurrentPiece(null);
                return;
            }

            var next = _pieces.Dequeue();
            MakeCurrentPiece(next);
        }

        private void MakeCurrentPiece(Piece piece)
        {
            CurrentPiece = piece;

            if (CurrentPiece != null)
            {
                CurrentPiece.Position = new Vector2Int(_width / 2, _height + 1);
            }

            UpdateShadow();

            if (CurrentPiece == null)
            {
                FinishGameEvent();
            }

            PieceSpawnedEvent(CurrentPiece);
        }

        public Dictionary<Vector2Int, Tile> Tiles()
        {
            return _placedTiles.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void PiecePlacedEvent(Piece piece)
        {
            OnPiecePlaced?.Invoke(piece);
        }

        private void PieceSpawnedEvent(Piece piece)
        {
            OnPieceSpawned?.Invoke(piece);
        }

        private void UpdateShadowEvent(Piece piece)
        {
            OnUpdateShadow?.Invoke(piece);
        }

        private void UpdateSwapEvent(Piece piece)
        {
            OnUpdateSwap?.Invoke(piece);
        }

        private void MovePieceEvent(Vector2Int direction)
        {
            OnMovePiece?.Invoke(direction);
        }

        private void RotatePieceEvent(int direction)
        {
            OnRotatePiece?.Invoke(direction);
        }

        private void PlacedTilesChanged(Dictionary<Vector2Int, Tile> changedTiles)
        {
            OnPlacedTilesChanged?.Invoke(changedTiles);
        }

        private void FinishGameEvent()
        {
            OnGameFinish?.Invoke();
        }
    }
}