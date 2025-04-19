using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace tetris
{
    public class PieceGenerator : MonoBehaviour
    {
        private int numberOfColors = 3;
        private int tilesPerPiece = 4;

        public Piece GeneratePiece(Vector2Int position)
        {
            var tiles = GenerateTiles();
            return new Piece(0, position, tiles);
        }

        private Dictionary<Vector2Int, Tile> GenerateTiles()
        {
            Vector2Int[] positions = GeneratePositions();
            Tile[] tiles = GenerateColors();

            var result = new Dictionary<Vector2Int, Tile>();
            for (int i = 0; i < tilesPerPiece; i++)
            {
                result.Add(positions[i], tiles[i]);
            }

            return result;
        }

        private Tile[] GenerateColors()
        {
            Tile[] result = new Tile[tilesPerPiece];
            int color = Random.Range(0, numberOfColors);

            for (int i = 0; i < tilesPerPiece; i++)
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