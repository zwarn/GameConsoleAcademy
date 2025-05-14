using System;
using System.Collections.Generic;
using UnityEngine;

namespace tetris
{
    public class PiecePreviewSystem : MonoBehaviour
    {
        [SerializeField] public List<PiecePreview> previews;
        [SerializeField] public PiecePreview swapPreview;
        [SerializeField] public TetrisController tetrisController;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = tetrisController.GetTetrisSystem();
            _tetrisSystem.OnPieceSpawned += UpdatePreview;
            _tetrisSystem.OnUpdateSwap += UpdateSwap;

            UpdatePreview(null);
        }

        private void UpdateSwap(Piece swap)
        {
            swapPreview.SetData(swap);
        }

        private void OnDestroy()
        {
            _tetrisSystem.OnPieceSpawned -= UpdatePreview;
            _tetrisSystem.OnUpdateSwap -= UpdateSwap;
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