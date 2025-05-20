using System;
using System.Collections.Generic;
using UnityEngine;

namespace tetris
{
    public class TetrisView : MonoBehaviour
    {
        [SerializeField] private TetrisController tetrisController;

        [SerializeField] private PieceView pieceView;
        [SerializeField] private TileView tileViewPrefab;
        [SerializeField] private Transform tileViewParent;
        [SerializeField] private GameObject background;
        [SerializeField] private Camera tetrisCamera;

        public Dictionary<Vector2Int, TileView> placedTiles = new();

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = tetrisController.GetTetrisSystem();

            _tetrisSystem.OnPieceSpawned += UpdateView;
            _tetrisSystem.OnPiecePlaced += PiecePlaced;
            UpdateView(_tetrisSystem.CurrentPiece);
        }

        private void OnDestroy()
        {
            _tetrisSystem.OnPieceSpawned -= UpdateView;
            _tetrisSystem.OnPiecePlaced -= PiecePlaced;
        }

        private void PiecePlaced(Piece piece)
        {
            var newTiles = piece.GetRotatedTranslatedTiles();
            foreach (var pair in newTiles)
            {
                var tileView = Instantiate(tileViewPrefab, tileViewParent);
                tileView.SetData(pair.Key, pair.Value.Color);
                placedTiles.Add(pair.Key, tileView);
            }
        }

        private void UpdateView(Piece piece)
        {
            pieceView.SetData(piece);
        }
    }
}