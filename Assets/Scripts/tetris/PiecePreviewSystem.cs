using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace tetris
{
    public class PiecePreviewSystem : MonoBehaviour
    {
        [SerializeField] public List<PiecePreview> previews;
        [SerializeField] private PiecePreview swapPreview;
        
        [Inject] private TetrisController _tetrisController;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = _tetrisController.GetTetrisSystem();
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
                if (nextPieces.Count <= i)
                {
                    previews[i].SetData(null);
                    return;
                }

                var nextPiece = nextPieces[i];
                nextPiece.Position = Vector2Int.zero;
                previews[i].SetData(nextPiece);
            }
        }
    }
}