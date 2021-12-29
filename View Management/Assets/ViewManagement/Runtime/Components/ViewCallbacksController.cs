using System;
using UnityEngine.Events;

namespace ViewManagement.Components
{
    [Serializable]
    public class ViewCallbacksController
    {
        public bool drawInitialize;
        public bool drawShow;
        public bool drawShown;
        public bool drawHide;
        public bool drawHidden;
        public bool drawExit;
        
        public UnityEvent onInitialize;
        public UnityEvent onShow;
        public UnityEvent onShown;
        public UnityEvent onHide;
        public UnityEvent onHidden;
        public UnityEvent onExit;
        public ViewCallbacks[] callbacks;

        public void OnInitialize()
        {
            foreach (ViewCallbacks initializer in callbacks)
            {
                initializer.OnInitialize();
            }
            
            if (!drawInitialize)
                return;
            
            onInitialize?.Invoke();
        }

        public void OnShow()
        {
            foreach (ViewCallbacks callback in callbacks)
            {
                callback.OnShow();
            }
            
            if (!drawShow)
                return;

            onShow?.Invoke();
        }

        public void OnShown()
        {
            foreach (ViewCallbacks callback in callbacks)
            {
                callback.OnShown();
            }

            if (!drawShown)
                return;

            onShown?.Invoke();
        }

        public void OnHide()
        {
            foreach (ViewCallbacks callback in callbacks)
            {
                callback.OnHide();
            }
            
            if (!drawHide)
                return;

            onHide?.Invoke();
        }

        public void OnHidden()
        {
            foreach (ViewCallbacks callback in callbacks)
            {
                callback.OnHidden();
            }
            
            if (!drawHidden)
                return;

            onHidden?.Invoke();
        }

        public void OnExit()
        {
            foreach (ViewCallbacks callback in callbacks)
            {
                callback.OnExit();
            }
            
            if (!drawExit)
                return;
            
            onExit?.Invoke();
        }
    }
}