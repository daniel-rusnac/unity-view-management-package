using UnityEngine;

namespace ViewManagement
{
    /// <summary>
    /// Has all the callbacks for a <see cref="View"/>.
    /// Inherit to create your custom view behaviours.
    /// </summary>
    public abstract class ViewCallbacks : MonoBehaviour
    {
        /// <summary>
        /// Fired once, in Awake from the ViewManager.
        /// </summary>
        public virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Fired every time the view is activated.
        /// </summary>
        public virtual void OnShow()
        {
        }

        /// <summary>
        /// Fired when the the finished all show animations.
        /// </summary>
        public virtual void OnShown()
        {
        }

        /// <summary>
        /// Fired when the view starts the hide animation.
        /// </summary>
        public virtual void OnHide()
        {
        }

        /// <summary>
        /// Fired when the view finished all hide animations and is disabled.
        /// </summary>
        public virtual void OnHidden()
        {
        }
        
        /// <summary>
        /// Fired when this is the only active view and 'Back Button' is pressed.
        /// </summary>
        public virtual void OnExit()
        {
        }
    }
}