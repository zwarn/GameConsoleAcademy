using System;
using System.Collections.Generic;
using tetris;
using UnityEngine;

public class PieceView : MonoBehaviour
{
    [SerializeField] private TileView tileViewPrefab;
    [SerializeField] private Transform tileViewParent;
    
    private List<TileView> _tileViews = new();
    private Piece _current;
    
    
    public void SetData(Piece piece)
    {
        _current = piece;
        Clear();
        foreach (var pair in piece.GetTiles())
        {
            var view = Instantiate(tileViewPrefab, tileViewParent);
            view.SetData(pair.Key, pair.Value.Color);
            _tileViews.Add(view);
        }
    }

    private void Clear()
    {
        foreach (var tileView in _tileViews)
        {
            Destroy(tileView.gameObject);
        }
        _tileViews.Clear();
    }

    private void Update()
    {
        var position = _current.Position;
        transform.position = new Vector3(position.x, position.y);

        var rotation = _current.Rotation;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0,90 * rotation));
    }
}
