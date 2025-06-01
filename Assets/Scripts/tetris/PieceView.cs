using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace tetris
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;

        [Inject] private TetrisColorRepository _colorRepository;

        private Piece _current;
        private Vector2Int _prevPosition;
        private int _prevRotation;

        private Tween _moveTween;
        private Tween _rotationTween;

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
                _colorRepository.AddToTilemap(tilemap, pair.Key, pair.Value.Color);
            }

            _prevPosition = _current.Position;
            _prevRotation = _current.Rotation;
            transform.localPosition = new Vector3(_prevPosition.x, _prevPosition.y, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 90 * _prevRotation);
            DoUpdate();
        }

        private void Clear()
        {
            tilemap.ClearAllTiles();
            _moveTween?.Kill();
            _rotationTween?.Kill();
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
            if (position != _prevPosition)
            {
                _moveTween?.Kill();
                var teleport = (_prevPosition - position).magnitude >= 2;
                _moveTween = transform.DOLocalMove(new Vector3(position.x, position.y), teleport ? 0 : 0.15f)
                    .SetEase(Ease.OutQuad);
                _prevPosition = position;
            }

            var rotation = _current.Rotation;
            if (rotation != _prevRotation)
            {
                _rotationTween?.Kill();
                _rotationTween = transform.DOLocalRotate(new Vector3(0, 0, 90 * rotation), 0.10f, RotateMode.Fast)
                    .SetEase(Ease.OutBack);
                _prevRotation = rotation;
            }
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
            _rotationTween?.Kill();
        }
    }
}