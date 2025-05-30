using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace tetris
{
    public class TetrisColorRepository : MonoBehaviour
    {
        public static TetrisColorRepository Instance;

        //TODO replace with DependencyInjection
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        [SerializeField] private List<ColorTileMatching> colorTileMatching;


        private TileBase ColorToTile(int color)
        {
            return colorTileMatching.Find(matching => matching.color == color).tile;
        }

        private Vector3Int ToVector3(Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y);
        }

        public void AddToTilemap(Tilemap tilemap, Vector2Int position, int color)
        {
            tilemap.SetTile(ToVector3(position), ColorToTile(color));
        }


        [Serializable]
        public class ColorTileMatching
        {
            public int color;
            public TileBase tile;
        }
    }
}