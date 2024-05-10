using DG.Tweening;
using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Tile
{
    public class TileAnimation
    {
        private Tweener _colorTween;
        private Tweener _scaleTween;
        private Tweener _positionTween;
        private readonly SpriteRenderer _renderer;
        private readonly Transform _transform;

        public TileAnimation(SpriteRenderer renderer, Transform transform)
        {
            _renderer = renderer;
            _transform = transform;
        }

        public void AnimateColor(Color newColor, float duration = 0.25f)
        {
            _colorTween = RestartOrCreateTimeTween(_colorTween, newColor, duration, target => _renderer.DOColor(target, duration));
        }

        public void AnimateScale(Vector3 targetScale, float duration = 0.25f)
        {
            _scaleTween = RestartOrCreateTimeTween(_scaleTween, targetScale, duration, target => _transform.DOScale(target, duration));
        }

        public void AnimatePosition(Vector3 targetPosition, float duration = 0.25f)
        {
            _positionTween = RestartOrCreateTimeTween(_positionTween, targetPosition, duration, target => _transform.DOLocalMove(target, duration));
        }

        private Tweener RestartOrCreateTimeTween<T>(Tweener tween, T endValue, float duration, System.Func<T, Tweener> createTweenFunc)
        {
            if (tween == null || !tween.IsActive() || !tween.IsPlaying())
                return createTweenFunc(endValue).SetAutoKill(false);
            tween.ChangeEndValue(endValue, duration, true).Restart();
            return tween;
        }

        public void OnDestroy()
        {
            _colorTween?.Kill();
            _scaleTween?.Kill();
            _positionTween?.Kill();
            DOTween.Kill(_transform);
        }
    }
}