using System;
using UnityEngine;
using Zenject;

namespace tetris
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip rotateClip;
        [SerializeField] private AudioClip quickDropClip;
        [SerializeField] private AudioClip lockClip;
        [SerializeField] private AudioClip swapClip;
        [SerializeField] private AudioClip undoClip;

        [Inject] private TetrisController _tetrisController;

        private TetrisSystem _tetrisSystem;


        private void Start()
        {
            _tetrisSystem = _tetrisController.GetTetrisSystem();

            _tetrisSystem.OnRotatePiece += PlayRotateSound;
            _tetrisSystem.OnPiecePlaced += PlayLockSound;
            _tetrisSystem.OnQuickdrop += PlayDropSound;
            _tetrisSystem.OnSwap += PlaySwapSound;
            _tetrisController.OnUndo += PlayUndoSound;
        }

        private void OnDestroy()
        {
            _tetrisSystem.OnRotatePiece -= PlayRotateSound;
            _tetrisSystem.OnPiecePlaced -= PlayLockSound;
            _tetrisSystem.OnQuickdrop -= PlayDropSound;
            _tetrisSystem.OnSwap -= PlaySwapSound;
            _tetrisController.OnUndo -= PlayUndoSound;
        }

        private void PlayRotateSound(int _)
        {
            AudioSource.PlayClipAtPoint(rotateClip, Vector3.zero);
        }

        private void PlayLockSound(Piece _)
        {
            AudioSource.PlayClipAtPoint(lockClip, Vector3.zero);
        }

        private void PlayDropSound()
        {
            AudioSource.PlayClipAtPoint(quickDropClip, Vector3.zero);
        }

        private void PlaySwapSound()
        {
            AudioSource.PlayClipAtPoint(swapClip, Vector3.zero);
        }

        private void PlayUndoSound()
        {
            AudioSource.PlayClipAtPoint(undoClip, Vector3.zero);
        }
    }
}