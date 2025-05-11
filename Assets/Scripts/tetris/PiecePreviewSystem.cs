using System;
using System.Collections.Generic;
using UnityEngine;

namespace tetris
{
    public class PiecePreviewSystem : MonoBehaviour
    {
        [SerializeField] public List<PiecePreview> previews;
        [SerializeField] public TetrisController tetrisController;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = tetrisController.GetTetrisSystem();
            _tetrisSystem.OnPieceSpawned += UpdatePreview;

            UpdatePreview(null);
        }

        private void UpdatePreview(Piece _)
        {
            var nextPieces = _tetrisSystem.NextPieces(previews.Count);
            for (int i = 0; i < previews.Count; i++)
            {
                previews[i].SetData(nextPieces.Count > i ? nextPieces[i] : null);
            }
        }
    }
}