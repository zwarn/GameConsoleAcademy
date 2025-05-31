using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace tetris
{
    public class RemainingTilesCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private int offset;
        
        [Inject] private TetrisController _tetrisController;

        private TetrisSystem _tetrisSystem;

        private void Start()
        {
            _tetrisSystem = _tetrisController.GetTetrisSystem();
        }

        private void Update()
        {
            var remainingPieces = _tetrisSystem.RemainingPieces();
            label.gameObject.SetActive(remainingPieces - offset > 0);
            label.SetText((remainingPieces - offset).ToString());
        }
    }
}