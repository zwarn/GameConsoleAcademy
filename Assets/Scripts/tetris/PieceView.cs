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
        
        if (_current == null)
        {
            return;
        }

        foreach (var pair in piece.GetTiles())
        {
            var view = Instantiate(tileViewPrefab, tileViewParent);
            view.SetData(pair.Key, pair.Value.Color);
            _tileViews.Add(view);
        }
        
        DoUpdate();
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