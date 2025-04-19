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

        private readonly int _width;
        private readonly int _height;
        private readonly Dictionary<Vector2Int, Tile> _placedTiles = new();
        private readonly PieceGenerator _pieceGenerator;

        public Piece CurrentPiece;
        public Piece NextPiece;

        public TetrisSystem(int width, int height, PieceGenerator pieceGenerator)
        {
            _width = width;
            _height = height;
            _pieceGenerator = pieceGenerator;
            CurrentPiece = GeneratePiece();
            NextPiece = GeneratePiece();
        }

        private bool IsInBounds(Vector2Int position)
        {
            return position.x >= 0 && position.x < _width && position.y >= 0 && position.y < _height;
        }

        private bool IsColliding(Piece piece)
        {
            return !piece.GetRotatedTranslatedTiles().Select(tile => tile.Key)
                .All(pos => IsInBounds(pos) && !_placedTiles.ContainsKey(pos));
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
            }
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
            }
        }

        private void LockPiece()
        {
            foreach (var pair in CurrentPiece.GetRotatedTranslatedTiles())
            {
                _placedTiles[pair.Key] = pair.Value;
            }

            PiecePlacedEvent(CurrentPiece);
            CurrentPiece = NextPiece;
            NextPiece = GeneratePiece();
            PieceSpawnedEvent(CurrentPiece);
        }

        private Piece GeneratePiece()
        {
            //TODO: check if generating at this position is possible otherwise shift position or end the game
            return _pieceGenerator.GeneratePiece(new Vector2Int(_width / 2, _height - 3));
        }

        protected virtual void PiecePlacedEvent(Piece piece)
        {
            OnPiecePlaced?.Invoke(piece);
        }

        protected virtual void PieceSpawnedEvent(Piece piece)
        {
            OnPieceSpawned?.Invoke(piece);
        }
    }
}