using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace tetris
{
    public class TetrisView : MonoBehaviour
    {

        [SerializeField] private PieceView pieceView;
        [SerializeField] private GameObject background;
        [SerializeField] private Camera tetrisCamera;
        [SerializeField] private Tilemap tilemap;
        
        [Inject] private TetrisController _tetrisController;
        [Inject] private TetrisColorRepository _colorRepository;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = _tetrisController.GetTetrisSystem();

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
                _colorRepository.AddToTilemap(tilemap, pair.Key, pair.Value.Color);
            }
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
                _colorRepository.AddToTilemap(tilemap, pair.Key, pair.Value.Color);
            }
        }

        private void UpdateView(Piece piece)
        {
            pieceView.SetData(piece);
        }
    }
}