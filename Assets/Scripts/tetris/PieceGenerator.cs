using System.Collections.Generic;
using UnityEngine;

namespace tetris
{
    public class PieceGenerator : MonoBehaviour
    {
        private int numberOfColors = 5;
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

            for (int i = 0; i < tilesPerPiece; i++)
            {
                int color = Random.Range(0, numberOfColors);
                result[i] = new Tile(color);
            }

            return result;
        }

        private Vector2Int[] GeneratePositions()
        {
            Vector2Int[] result = new Vector2Int[tilesPerPiece];

            result[0] = new Vector2Int(0, 0);
            result[1] = new Vector2Int(0, 1);
            result[2] = new Vector2Int(1, 1);
            result[3] = new Vector2Int(1, 0);

            return result;
        }
    }
}