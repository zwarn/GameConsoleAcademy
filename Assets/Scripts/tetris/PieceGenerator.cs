using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace tetris
{
    public class PieceGenerator : MonoBehaviour
    {
        [SerializeField] private List<int> colorGenerationWeight = new List<int> { 0, 60, 30, 10 };

        private readonly int _tilesPerPiece = 4;

        public List<Piece> GeneratePieces(int amount)
        {
            List<Piece> result = new List<Piece>();

            for (int i = 0; i < amount; i++)
            {
                result.Add(GeneratePiece());
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
            int[] candidateColors = ChooseNumberOfMixedColors();
            int color = candidateColors[Random.Range(0, candidateColors.Length)];

            for (int i = 0; i < _tilesPerPiece; i++)
            {
                result[i] = new Tile(color);
            }

            return result;
        }

        private int[] ChooseNumberOfMixedColors()
        {
            int max = colorGenerationWeight.Sum();
            var random = Random.Range(0, max);

            int accumulator = colorGenerationWeight[0];

            if (random < accumulator)
            {
                return new[] { 0 };
            }

            accumulator += colorGenerationWeight[1];

            if (random < accumulator)
            {
                return new[] { 1, 2, 3 };
            }

            accumulator += colorGenerationWeight[2];

            if (random < accumulator)
            {
                return new[] { 4, 5, 6 };
            }

            return new[] { 7 };
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
                TetrisShape.L => new Vector2Int[] { new(0, 0), new(1, 0), new(-1, 0), new(-1, 1) },
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