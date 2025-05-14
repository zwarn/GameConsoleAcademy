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
        public event Action<Piece> OnUpdateShadow;
        public event Action<Piece> OnUpdateSwap;

        private readonly int _width;
        private readonly int _height;
        private readonly int _limit;
        private readonly Dictionary<Vector2Int, Tile> _placedTiles = new();
        private Queue<Piece> _pieces;

        public Piece CurrentPiece;
        public Piece CurrentShadow;
        public Piece CurrentSwap;
        public bool Finished = false;

        public TetrisSystem(int width, int height, Queue<Piece> pieces)
        {
            _width = width;
            _height = height;
            _limit = height;
            _pieces = pieces;
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

        public void Move(Vector2Int direction)
        {
            if (CurrentPiece == null)
            {
                return;
            }

            var copy = CurrentPiece.Copy();
            copy.Move(direction);

            if (!IsColliding(copy))
            {
                CurrentPiece.Move(direction);
                UpdateShadow();
            }
        }

        public void Rotate(int direction)
        {
            if (CurrentPiece == null)
            {
                return;
            }

            var copy = CurrentPiece.Copy();
            copy.Rotate(direction);

            if (!IsColliding(copy))
            {
                CurrentPiece.Rotate(direction);
                UpdateShadow();
            }
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

            var copy = CurrentPiece.Copy();
            copy.Move(Vector2Int.down);

            if (IsColliding(copy))
            {
                LockPiece();
            }
            else
            {
                CurrentPiece.Move(Vector2Int.down);
                UpdateShadow();
            }
        }

        private void LockPiece()
        {
            foreach (var pair in CurrentPiece.GetRotatedTranslatedTiles())
            {
                _placedTiles[pair.Key] = pair.Value;
                if (pair.Key.y >= _limit)
                {
                    Finished = true;
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
            }

            var next = _pieces.Dequeue();
            MakeCurrentPiece(next);
        }

        private void MakeCurrentPiece(Piece piece)
        {
            CurrentPiece = piece;

            if (CurrentPiece != null)
            {
                CurrentPiece.Position = new Vector2Int(_width / 2, _limit + 1);
            }

            UpdateShadow();

            if (CurrentPiece == null)
            {
                Finished = true;
            }

            PieceSpawnedEvent(CurrentPiece);
        }

        protected virtual void PiecePlacedEvent(Piece piece)
        {
            OnPiecePlaced?.Invoke(piece);
        }

        protected virtual void PieceSpawnedEvent(Piece piece)
        {
            OnPieceSpawned?.Invoke(piece);
        }

        protected virtual void UpdateShadowEvent(Piece piece)
        {
            OnUpdateShadow?.Invoke(piece);
        }

        protected virtual void UpdateSwapEvent(Piece piece)
        {
            OnUpdateSwap?.Invoke(piece);
        }
    }
}