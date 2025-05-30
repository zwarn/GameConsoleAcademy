using System;
using tetris;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PieceView : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TetrisColorRepository colorRepository;

    private TetrisColorRepository GetColorRepository()
    {
        if (colorRepository == null)
        {
            colorRepository = TetrisColorRepository.Instance;
        }

        return colorRepository;
    }

    private Piece _current;

    public void SetData(Piece piece)
    {
        _current = piece;
        Clear();

        if (_current == null)
        {
            return;
        }

        foreach (var pair in piece.GetTiles())
        {
            GetColorRepository().AddToTilemap(tilemap, pair.Key, pair.Value.Color);
        }

        DoUpdate();
    }

    private void Clear()
    {
        tilemap.ClearAllTiles();
    }

    private void Update()
    {
        DoUpdate();
    }

    private void DoUpdate()
    {
        if (_current == null)
        {
            return;
        }

        var position = _current.Position;
        transform.localPosition = new Vector3(position.x, position.y);

        var rotation = _current.Rotation;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * rotation));
    }
}