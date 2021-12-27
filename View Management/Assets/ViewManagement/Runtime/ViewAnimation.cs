using System;
using UnityEngine;

namespace TryMyGames.ViewManagement
{
    [RequireComponent(typeof(View))]
    public abstract class ViewAnimation : MonoBehaviour
    {
        public void Show(Action onComplete)
        {
            OnStop();
            OnShow(onComplete);
        }

        public void Hide(Action onComplete)
        {
            OnStop();
            OnHide(onComplete);
        }

        protected abstract void OnShow(Action onComplete);
        protected abstract void OnHide(Action onComplete);
        protected abstract void OnStop();
    }
}