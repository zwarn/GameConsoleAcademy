using UnityEngine;

namespace tetris
{
    public class PiecePreview : MonoBehaviour
    {
        [SerializeField] private PieceView pieceView;


        public void SetData(Piece piece)
        {
            pieceView.SetData(piece);
        }
    }
}