using UnityEngine;

namespace tetris
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color[] colors;

        public void SetData(Vector2Int position, int color)
        {
            transform.localPosition = new Vector3(position.x, position.y);
            spriteRenderer.color = colors[color];
        }
    }
}