using System;
using DG.Tweening;
using UnityEngine;

namespace ViewManagement.Samples.SampleAnimations
{
    public class CascadeViewAnimationDotween : ViewAnimation
    {
        [SerializeField] private float showDuration = 0.3f;
        [SerializeField] private float hideDuration;
        [SerializeField] private float interval = 0.05f;
        [SerializeField] private Transform[] targets;
        private Tween tween;

        protected override void OnShow(Action onComplete)
        {
            tween = AnimateChildrenScale(0f, 1f, showDuration, Ease.OutBack).SetUpdate(true)
                .OnComplete(() => onComplete?.Invoke());
        }

        protected override void OnHide(Action onComplete)
        {
            if (hideDuration == 0)
            {
                onComplete?.Invoke();
                return;
            }

            tween = AnimateChildrenScale(1f, 0f, hideDuration, Ease.OutQuad).SetUpdate(true)
                .OnComplete(() => onComplete?.Invoke());
        }

        protected override void OnStop()
        {
            tween?.Kill();
        }

        private Tween AnimateChildrenScale(float from, float to, float duration, Ease ease)
        {
            Sequence sequence = DOTween.Sequence();
            float time = 0f;
            foreach (Transform child in targets)
            {
                child.localScale = Vector3.one * from;
                sequence.Insert(time, child.DOScale(to, duration).SetEase(ease));
                time += interval;
            }

            return sequence.Play();
        }
    }
}