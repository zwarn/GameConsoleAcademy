using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace tetris
{
    public class TetrisLayerVisualizer : MonoBehaviour
    {
        [SerializeField] private Tilemap tileMapRed;
        [SerializeField] private Tilemap tileMapBlue;
        [SerializeField] private Tilemap tileMapYellow;
        [SerializeField] private TileBase borderTile;
        
        [Inject] private TetrisController _tetrisController;
        [Inject] private TetrisColorRepository _colorRepository;
        
        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = _tetrisController.GetTetrisSystem();
            _tetrisSystem.OnPiecePlaced += PiecePlaced;
            _tetrisSystem.OnPlacedTilesChanged += ReloadPlacedTiles;
        }

        private void OnDestroy()
        {
            _tetrisSystem.OnPiecePlaced -= PiecePlaced;
            _tetrisSystem.OnPlacedTilesChanged -= ReloadPlacedTiles;
        }

        private void PiecePlaced(Piece piece)
        {
            var newTiles = piece.GetRotatedTranslatedTiles();
            AddTiles(newTiles);
        }

        private void AddTiles(Dictionary<Vector2Int, Tile> tiles)
        {
            foreach (var pair in tiles)
            {
                int color = pair.Value.Color;
                if (_colorRepository.IsRed(color))
                {
                    tileMapRed.SetTile(new Vector3Int(pair.Key.x, pair.Key.y), borderTile);
                }

                if (_colorRepository.IsBlue(color))
                {
                    tileMapBlue.SetTile(new Vector3Int(pair.Key.x, pair.Key.y), borderTile);
                }

                if (_colorRepository.IsYellow(color))
                {
                    tileMapYellow.SetTile(new Vector3Int(pair.Key.x, pair.Key.y), borderTile);
                }
            }
        }

        private void ReloadPlacedTiles(Dictionary<Vector2Int, Tile> tiles)
        {
            Clear();

            AddTiles(tiles);
        }

        private void Clear()
        {
            tileMapRed.ClearAllTiles();
            tileMapBlue.ClearAllTiles();
            tileMapYellow.ClearAllTiles();
        }
    }
}