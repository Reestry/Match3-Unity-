// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using DG.Tweening;
using Pool;

namespace Tokens
{
    public abstract class Token : MonoBehaviour, IReleasable
    {
        private readonly float _duration = 1f;
        private readonly float _strength = 0.05f;
        private readonly int _vibrato = 5;
        private readonly float _randomness = 90f;
        private readonly float _colorDuration = 0.3f;

        protected SpriteRenderer SpriteRenderer;
        protected string Name;
        protected int Score;
        protected int Index;

        private Tweener _shakeTween;
        private Vector3 _fallDistance = new(0, 0.8f, 0);

        private void OnEnable()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Jump(Vector3 position)
        {
            transform.position = position + _fallDistance;
            var color = SpriteRenderer.color;
            color.a = 0f;
            SpriteRenderer.color = color;

            SpriteRenderer.DOColor(new Color(color.r, color.g, color.b, 1f), _colorDuration).SetEase(Ease.InQuart)
                .SetAutoKill(true);
            transform.DOMove(position, _duration).SetEase(Ease.OutBounce).SetAutoKill(true);
        }

        public void ShakeAnimation()
        {
            _shakeTween = transform.DOShakePosition(_duration, _strength, _vibrato, _randomness)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.InOutQuad);
        }

        public void StopShakeAnimation(Transform tile)
        {
            if (_shakeTween != null)
                _shakeTween.Kill();
            
            transform.DOMove(tile.transform.position, 0.1f).SetEase(Ease.Flash).SetAutoKill(true);
        }

        public string GetName()
        {
            return Name;
        }

        public int GetScore()
        {
            return Score;
        }

        public int GetIndex()
        {
            return Index;
        }
        
        public virtual void ActivateEffect()
        {
        }

        private void OnDisable()
        {
            _shakeTween.Kill();
        }

        public virtual void OnRelease()
        {
            var explosion = PoolObject.Get<TokenExplosion>(); 
            explosion.transform.position = transform.position; 
        }
    }
}