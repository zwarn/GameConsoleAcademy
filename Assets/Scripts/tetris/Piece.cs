using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace tetris
{
    public class Piece
    {
        public int Rotation { get; private set;}
        public Vector2Int Position { get; set; }

        private readonly Dictionary<Vector2Int, Tile>[] _rotatedTiles;

        public Piece(int rotation, Vector2Int position, Dictionary<Vector2Int, Tile> tiles)
        {
            Rotation = rotation;
            Position = position;
            _rotatedTiles = new[]
            {
                tiles,
                tiles.ToDictionary(tile => new Vector2Int(-tile.Key.y, tile.Key.x), tile => tile.Value),
                tiles.ToDictionary(tile => new Vector2Int(-tile.Key.x, -tile.Key.y), tile => tile.Value),
                tiles.ToDictionary(tile => new Vector2Int(tile.Key.y, -tile.Key.x), tile => tile.Value),
            };
        }

        public void Move(Vector2Int dir)
        {
            Position += dir;
        }

        public void Rotate(int dir)
        {
            var rotation = Rotation + dir;
            if (rotation < 0)
            {
                rotation += 4;
            }

            if (rotation > 3)
            {
                rotation -= 4;
            }

            Rotation = rotation;
        }

        public Dictionary<Vector2Int, Tile> GetTiles()
        {
            return _rotatedTiles[0];
        }

        public Dictionary<Vector2Int, Tile> GetRotatedTiles()
        {
            return _rotatedTiles[Rotation];
        }
        
        public Dictionary<Vector2Int, Tile> GetRotatedTranslatedTiles()
        {
            return _rotatedTiles[Rotation].ToDictionary(tile => tile.Key + Position, tile => tile.Value);
        }
    }
}