using System;
using TMPro;
using UnityEngine;

namespace tetris
{
    public class RemainingTilesCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private TetrisController tetrisController;
        [SerializeField] private int offset;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = tetrisController.GetTetrisSystem();
        }

        private void Update()
        {
            var remainingPieces = _tetrisSystem.RemainingPieces();
            label.gameObject.SetActive(remainingPieces - offset > 0);
            label.SetText((remainingPieces - offset).ToString());
        }
    }
}