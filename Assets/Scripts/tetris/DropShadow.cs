using System;
using UnityEngine;
using Zenject;

namespace tetris
{
    public class DropShadow : MonoBehaviour
    {
        [SerializeField] private PieceView shadow;
        
        [Inject] private TetrisController _tetrisController;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = _tetrisController.GetTetrisSystem();
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