using System;
using UnityEngine;

namespace ViewManagement
{
    /// <summary>
    /// Inherit to create your custom <see cref="View"/> animations.
    /// </summary>
    [RequireComponent(typeof(View))]
    public abstract class ViewAnimation : MonoBehaviour
    {
        /// <summary>
        /// Called by the view when 'show animation' starts. 
        /// </summary>
        /// <param name="onComplete"> MUST be called when the animation is done.</param>
        public void Show(Action onComplete)
        {
            OnStop();
            OnShow(onComplete);
        }

        /// <summary>
        /// Called by the view when 'hide animation' starts. 
        /// </summary>
        /// <param name="onComplete"> MUST be called when the animation is done.</param>
        public void Hide(Action onComplete)
        {
            OnStop();
            OnHide(onComplete);
        }

        /// <summary>
        /// Called by the view when 'show animation' starts. 
        /// </summary>
        protected abstract void OnShow(Action onComplete);
        
        /// <summary>
        /// Called by the view when 'hide animation' starts. 
        /// </summary>
        protected abstract void OnHide(Action onComplete);
        
        /// <summary>
        /// Called when an animation is interrupted.
        /// </summary>
        protected abstract void OnStop();
    }
}