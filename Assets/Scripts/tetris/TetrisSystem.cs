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

        public Piece CurrentPiece;
        public Piece NextPiece;

        public TetrisSystem(int width, int height)
        {
            _width = width;
            _height = height;
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
            
            CurrentPiece.Move(direction);
            if (IsColliding(CurrentPiece))
            {
                CurrentPiece.Move(new Vector2Int(-direction.x, -direction.y));
            }
        }

        public void Rotate(int direction)
        {
            if (CurrentPiece == null)
            {
                return;
            }
            
            CurrentPiece.Rotate(direction);
            if (IsColliding(CurrentPiece))
            {
                CurrentPiece.Rotate(-direction);
            }
        }

        public void Drop()
        {
            CurrentPiece.Move(Vector2Int.down);
            if (IsColliding(CurrentPiece))
            {
                CurrentPiece.Move(Vector2Int.up);
                LockPiece();
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
            var tiles = new Dictionary<Vector2Int, Tile>()
            {
                {new Vector2Int(0,0), new Tile(0)},
                {new Vector2Int(1,0), new Tile(0)},
                {new Vector2Int(1,1), new Tile(1)},
                {new Vector2Int(0,1), new Tile(1)},
            };
            return new Piece(0, new Vector2Int(_width / 2, _height - 3), tiles);
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