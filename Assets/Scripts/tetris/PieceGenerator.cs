using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace tetris
{
    public class PieceGenerator : MonoBehaviour
    {
        private readonly int _numberOfColors = 3;
        private readonly int _tilesPerPiece = 4;

        public Queue<Piece> GeneratePieces(int amount)
        {
            Queue<Piece> result = new Queue<Piece>();

            for (int i = 0; i < amount; i++)
            {
                result.Enqueue(GeneratePiece());
            }

            return result;
        }

        public Piece GeneratePiece()
        {
            var tiles = GenerateTiles();
            return new Piece(0, new Vector2Int(0, 0), tiles);
        }

        private Dictionary<Vector2Int, Tile> GenerateTiles()
        {
            Vector2Int[] positions = GeneratePositions();
            Tile[] tiles = GenerateColors();

            var result = new Dictionary<Vector2Int, Tile>();
            for (int i = 0; i < _tilesPerPiece; i++)
            {
                result.Add(positions[i], tiles[i]);
            }

            return result;
        }

        private Tile[] GenerateColors()
        {
            Tile[] result = new Tile[_tilesPerPiece];
            int color = Random.Range(0, _numberOfColors);

            for (int i = 0; i < _tilesPerPiece; i++)
            {
                result[i] = new Tile(color);
            }

            return result;
        }

        private Vector2Int[] GeneratePositions()
        {
            var shapes = Enum.GetValues(typeof(TetrisShape));
            var shape = (TetrisShape)shapes.GetValue(Random.Range(0, shapes.Length));
            return GeneratePositions(shape);
        }

        private Vector2Int[] GeneratePositions(TetrisShape shape)
        {
            return shape switch
            {
                TetrisShape.I => new Vector2Int[] { new(0, 0), new(-1, 0), new(1, 0), new(2, 0) },
                TetrisShape.O => new Vector2Int[] { new(0, 0), new(1, 0), new(1, 1), new(0, 1) },
                TetrisShape.L => new Vector2Int[] { new(0, 0), new(-1, 0), new(-2, 0), new(0, 1) },
                TetrisShape.J => new Vector2Int[] { new(0, 0), new(1, 0), new(2, 0), new(0, 1) },
                TetrisShape.S => new Vector2Int[] { new(0, 0), new(-1, 0), new(0, 1), new(1, 1) },
                TetrisShape.Z => new Vector2Int[] { new(0, 0), new(1, 0), new(0, 1), new(-1, 1) },
                TetrisShape.T => new Vector2Int[] { new(0, 0), new(1, 0), new(0, 1), new(-1, 0) },
            };
        }

        private enum TetrisShape
        {
            I,
            O,
            L,
            J,
            S,
            Z,
            T,
        }
    }
}