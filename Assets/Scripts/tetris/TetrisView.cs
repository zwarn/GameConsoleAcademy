using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace tetris
{
    public class TetrisView : MonoBehaviour
    {
        [SerializeField] private TetrisController tetrisController;

        [SerializeField] private PieceView pieceView;
        [SerializeField] private GameObject background;
        [SerializeField] private Camera tetrisCamera;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private List<ColorTileMatching> colorTileMatching;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = tetrisController.GetTetrisSystem();

            _tetrisSystem.OnPieceSpawned += UpdateView;
            _tetrisSystem.OnPiecePlaced += PiecePlaced;
            _tetrisSystem.OnPlacedTilesChanged += ReloadPlacedTiles;
            UpdateView(_tetrisSystem.CurrentPiece);
        }

        private void OnDestroy()
        {
            _tetrisSystem.OnPieceSpawned -= UpdateView;
            _tetrisSystem.OnPiecePlaced -= PiecePlaced;
            _tetrisSystem.OnPlacedTilesChanged -= ReloadPlacedTiles;
        }

        private void ReloadPlacedTiles(Dictionary<Vector2Int, Tile> tiles)
        {
            Clear();

            foreach (var pair in tiles)
            {
                tilemap.SetTile(ToVector3(pair.Key), ColorToTile(pair.Value.Color));
            }
        }

        private TileBase ColorToTile(int color)
        {
            return colorTileMatching.Find(matching => matching.color == color).tile;
        }

        private Vector3Int ToVector3(Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y);
        }

        private void Clear()
        {
            tilemap.ClearAllTiles();
        }

        private void PiecePlaced(Piece piece)
        {
            var newTiles = piece.GetRotatedTranslatedTiles();
            foreach (var pair in newTiles)
            {
                tilemap.SetTile(ToVector3(pair.Key), ColorToTile(pair.Value.Color));
            }
        }

        private void UpdateView(Piece piece)
        {
            pieceView.SetData(piece);
        }

        [Serializable]
        public class ColorTileMatching
        {
            public int color;
            public TileBase tile;
        }
    }
}