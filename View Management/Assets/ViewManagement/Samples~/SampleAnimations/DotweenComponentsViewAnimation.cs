using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TryMyGames.ViewManagement;
using UnityEditor;
using UnityEngine;

namespace ViewManagement.Samples.SampleAnimations
{
    public class DotweenComponentsViewAnimation : ViewAnimation
    {
        [SerializeField] private float maxShowDuration = 3f;
        [SerializeField] private float maxHideDuration = 1f;
        [SerializeField] private List<DOTweenAnimation> tweens = new List<DOTweenAnimation>();

        private Tween actionTween;
        private float maxTweenDuration;

        private void Awake()
        {
            maxTweenDuration = Mathf.Min(tweens.Max(tweenAnimation => tweenAnimation.delay + tweenAnimation.duration));
        }

        protected override void OnShow(Action onComplete)
        {
            for (int i = 0; i < tweens.Count; i++)
            {
                tweens[i].DORestart();
            }

            actionTween = DOVirtual.DelayedCall(Mathf.Min(maxShowDuration, maxTweenDuration), () => onComplete?.Invoke()).SetUpdate(true);
        }

        protected override void OnHide(Action onComplete)
        {
            float maxTime = 0f;

            for (int i = 0; i < tweens.Count; i++)
            {
                tweens[i].DOPlayBackwards();

                float currentTime = tweens[i].delay + tweens[i].duration;

                if (currentTime > maxTime)
                {
                    maxTime = currentTime;
                }
            }

            maxTime = Mathf.Min(maxHideDuration, maxTime);

            actionTween = DOVirtual.DelayedCall(maxTime, () => onComplete?.Invoke()).SetUpdate(true);
        }

        protected override void OnStop()
        {
            actionTween?.Kill();
        }

#if UNITY_EDITOR
        [ContextMenu("Load tweens in children")]
        private void GetTweens()
        {
            Undo.RecordObject(this, "Load View Tweens");
            tweens.Clear();
            DOTweenAnimation[] childTweens = GetComponentsInChildren<DOTweenAnimation>(true);

            for (int i = 0; i < childTweens.Length; i++)
            {
                if (childTweens[i].autoKill == false && childTweens[i].autoPlay == false &&
                    !tweens.Contains(childTweens[i]))
                {
                    childTweens[i].isIndependentUpdate = true;
                    tweens.Add(childTweens[i]);
                }
            }

            EditorUtility.SetDirty(this);
        }

        [ContextMenu("Clear tweens")]
        private void Clear()
        {
            Undo.RecordObject(this, "Clear View Tweens");
            tweens.Clear();
            EditorUtility.SetDirty(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            foreach (DOTweenAnimation tween in tweens)
            {
                if (tween == null)
                {
                    GetTweens();
                    return;
                }
                
                if (tween.TryGetComponent(out RectTransform rectTransform))
                {
                    Rect rect = rectTransform.rect;
                    rect.position = rectTransform.position - (Vector3) rect.size / 2;

                    Handles.DrawSolidRectangleWithOutline(rect, new Color(1f, 0.92f, 0.02f, 0.04f), Color.yellow);
                }
            }
        }
#endif
    }
}