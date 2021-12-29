using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViewManagement.Components
{
    public class CascadeViewAnimation : ViewAnimation
    {
        private const float SHOW_SCALE = 1f;
        private const float HIDE_SCALE = 0f;
        
        [SerializeField] private float showDuration = 0.3f;
        [SerializeField] private float hideDuration = 0.1f;
        [SerializeField] private float interval = 0.1f;
        [SerializeField] private AnimationCurve showEase = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private AnimationCurve hideEase = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private Transform[] targets;

        private Coroutine scaleCoroutine;
        private List<Coroutine> elementScaleCoroutines = new List<Coroutine>();

        protected override void OnShow(Action onComplete)
        {
            SetAllElementsScale(HIDE_SCALE);
            scaleCoroutine = StartCoroutine(DoScale(HIDE_SCALE, SHOW_SCALE, showDuration, showEase, onComplete));
        }
        
        protected override void OnHide(Action onComplete)
        {
            if (hideDuration == 0)
            {
                onComplete?.Invoke();
                return;
            }

            SetAllElementsScale(SHOW_SCALE);
            scaleCoroutine = StartCoroutine(DoScale(SHOW_SCALE, HIDE_SCALE, hideDuration, hideEase, onComplete));
        }
        
        protected override void OnStop()
        {
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);

                foreach (Coroutine elementScaleCoroutine in elementScaleCoroutines)
                {
                    StopCoroutine(elementScaleCoroutine);
                }
                
                elementScaleCoroutines.Clear();
            }
        }

        private void SetAllElementsScale(float scale)
        {
            foreach (Transform target in targets)
            {
                target.localScale = Vector3.one * scale;
            }
        }

        private IEnumerator DoScale(float from, float to, float duration, AnimationCurve ease, Action onComplete)
        {
            WaitForSeconds intervalWait = new WaitForSeconds(interval);
            Coroutine lastCoroutine = null;

            foreach (Transform child in targets)
            {
                lastCoroutine = StartCoroutine(DoScale(from, to, duration, ease, child));
                elementScaleCoroutines.Add(lastCoroutine);

                yield return intervalWait;
            }

            if (lastCoroutine != null)
            {
                yield return lastCoroutine;
            }

            onComplete?.Invoke();
        }

        private IEnumerator DoScale(float from, float to, float duration, AnimationCurve ease, Transform target)
        {
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                target.localScale = Vector3.one * Mathf.Lerp(from, to, ease.Evaluate(t));

                yield return null;
            }

            target.localScale = Vector3.one * to;
        }
    }
}