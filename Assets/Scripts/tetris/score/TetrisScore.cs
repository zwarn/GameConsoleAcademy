using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace tetris.score
{
    public static class TetrisScore
    {
        public static int Score(Dictionary<Vector2Int, Tile> tiles, int width, int height)
        {
            Tile[,] tilesArray = ConvertTiles(tiles, width, height);

            List<TetrisGroup> groups = ConvertToGroups(tilesArray);

            var groupedByType = groups
                .GroupBy(g => g.Type)
                .ToDictionary(g => g.Key, g => g.ToList());

            var numberOfEmptyEnclosedGroups = groupedByType[TetrisGroupType.Empty]
                .Count(group => group.GetTiles().All(pos => pos.y != height - 1));
            var numberOfRedGroups = groupedByType[TetrisGroupType.Red].Count;
            var numberOfBlueGroups = groupedByType[TetrisGroupType.Blue].Count;
            var numberOfYellowGroups = groupedByType[TetrisGroupType.Yellow].Count;

            return 13 - numberOfEmptyEnclosedGroups - numberOfRedGroups - numberOfBlueGroups - numberOfYellowGroups;
        }

        private static List<TetrisGroup> ConvertToGroups(Tile[,] tilesArray)
        {
            List<TetrisGroup> groups = new List<TetrisGroup>();
            List<Vector2Int> visited = new List<Vector2Int>();

            for (int x = 0; x < tilesArray.GetLength(0); x++)
            {
                for (int y = 0; y < tilesArray.GetLength(1); y++)
                {
                    var current = new Vector2Int(x, y);
                    if (visited.Contains(current))
                    {
                        continue;
                    }

                    var type = DetermineType(tilesArray[x, y]);
                    groups.Add(FillGroup(tilesArray, type, x, y, visited));
                }
            }

            return groups;
        }

        private static TetrisGroup FillGroup(Tile[,] tilesArray, TetrisGroupType type, int x, int y,
            List<Vector2Int> visited)
        {
            List<Vector2Int> tiles = new List<Vector2Int>();

            var startPosition = new Vector2Int(x, y);
            tiles.Add(startPosition);
            visited.Add(startPosition);

            Queue<Vector2Int> candidates = new Queue<Vector2Int>();
            FindCandidates(tilesArray, type, visited, new Vector2Int(x, y), candidates);

            while (candidates.Count > 0)
            {
                var position = candidates.Dequeue();

                tiles.Add(position);
                visited.Add(position);

                FindCandidates(tilesArray, type, visited, position, candidates);
            }

            return new TetrisGroup(type, tiles);
        }

        private static void FindCandidates(Tile[,] tilesArray, TetrisGroupType type, List<Vector2Int> visited,
            Vector2Int position,
            Queue<Vector2Int> candidates)
        {
            foreach (var candidate in Neighbors(tilesArray, position.x, position.y).Where(pos => !visited.Contains(pos))
                         .Where(pos => !candidates.Contains(pos))
                         .Where(pos => DetermineType(tilesArray[pos.x, pos.y]) == type))
            {
                candidates.Enqueue(candidate);
            }
        }

        private static List<Vector2Int> Neighbors(Tile[,] tiles, int x, int y)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);

            if (x > 0)
            {
                result.Add(new Vector2Int(x - 1, y));
            }

            if (y > 0)
            {
                result.Add(new Vector2Int(x, y - 1));
            }

            if (x < width - 1)
            {
                result.Add(new Vector2Int(x + 1, y));
            }

            if (y < height - 1)
            {
                result.Add(new Vector2Int(x, y + 1));
            }

            return result;
        }

        private static TetrisGroupType DetermineType(Tile tile)
        {
            if (tile == null)
            {
                return TetrisGroupType.Empty;
            }

            if (tile.Color == 0)
            {
                return TetrisGroupType.Red;
            }

            if (tile.Color == 1)
            {
                return TetrisGroupType.Blue;
            }

            if (tile.Color == 2)
            {
                return TetrisGroupType.Yellow;
            }

            throw new Exception($"Unexpected color for tile {tile}");
        }

        private static Tile[,] ConvertTiles(Dictionary<Vector2Int, Tile> tiles, int width, int height)
        {
            var result = new Tile[width, height];

            tiles.ToList().ForEach(pair =>
            {
                Vector2Int pos = pair.Key;
                if (pos.x < width && pos.y < height)
                {
                    result[pos.x, pos.y] = pair.Value;
                }
            });

            return result;
        }
    }

    public enum TetrisGroupType
    {
        Empty,
        Red,
        Yellow,
        Blue,
    }

    public class TetrisGroup
    {
        private readonly List<Vector2Int> _tiles;

        public TetrisGroupType Type { get; }
        public List<Vector2Int> GetTiles() => _tiles.ToList();
        public int Count => _tiles.Count;

        public TetrisGroup(TetrisGroupType type, List<Vector2Int> tiles)
        {
            Type = type;
            _tiles = tiles;
        }
    }
}