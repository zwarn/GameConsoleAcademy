using System.Collections.Generic;
using UnityEngine;

namespace tetris
{
    public class TetrisState
    {
        public readonly int Width;
        public readonly int Height;
        public readonly List<Piece> UpcomingPieces;
        public readonly Piece Swap;
        public readonly Dictionary<Vector2Int, Tile> PlacedTiles;

        public TetrisState(int width, int height, List<Piece> upcomingPieces, Piece swap,
            Dictionary<Vector2Int, Tile> placedTiles)
        {
            Width = width;
            Height = height;
            UpcomingPieces = upcomingPieces;
            Swap = swap;
            PlacedTiles = placedTiles;
        }
    }
}