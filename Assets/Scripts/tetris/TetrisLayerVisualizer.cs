using System;
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
        private int _showLayer = 0;

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

        public void SetLayer(int layer)
        {
            _showLayer = layer;
        }

        public void ShiftLayer(int delta)
        {
            _showLayer += delta;
            if (_showLayer < 0)
            {
                _showLayer += 4;
            }

            if (_showLayer > 3)
            {
                _showLayer -= 4;
            }
        }
        
        private void Update()
        {
            tileMapRed.gameObject.SetActive(_showLayer == 1);
            tileMapBlue.gameObject.SetActive(_showLayer == 2);
            tileMapYellow.gameObject.SetActive(_showLayer == 3);
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