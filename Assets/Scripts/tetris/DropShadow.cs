using System;
using UnityEngine;

namespace tetris
{
    public class DropShadow : MonoBehaviour
    {
        [SerializeField] private TetrisController tetrisController;
        [SerializeField] private PieceView shadow;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = tetrisController.GetTetrisSystem();
            _tetrisSystem.OnUpdateShadow += UpdateShadow;
            UpdateShadow(_tetrisSystem.CurrentShadow);
        }

        private void OnDestroy()
        {
            _tetrisSystem.OnUpdateShadow -= UpdateShadow;
        }

        private void UpdateShadow(Piece piece)
        {
            shadow.SetData(piece);
        }
    }
}