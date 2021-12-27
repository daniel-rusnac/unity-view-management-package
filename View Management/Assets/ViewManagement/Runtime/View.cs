using System;
using TryMyGames.SOArchitecture.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace TryMyGames.ViewManagement
{
    public class View : MonoBehaviour
    {
        [SerializeField] private int depth;
        [SerializeField] private VoidChannelSO showEvent;
        [SerializeField] private ViewMode mode;
        [SerializeField] private UnityEvent onExit;

        private int animationsCompleted;
        private bool isShown;
        private bool isLocked;
        private ViewAnimation[] toggleAnimations;
        private ViewCallbacks[] viewListeners;

        public event Action onShown;
        public event Action onHidden;
        public int Depth => depth;
        public VoidChannelSO ShowEvent => showEvent;
        public ViewMode Mode => mode;
        public bool IsShown => isShown;
        public bool IsLocked => isLocked;

        public void Initialize()
        {
            isShown = gameObject.activeSelf;
            viewListeners = GetComponents<ViewCallbacks>();
            toggleAnimations = GetComponents<ViewAnimation>();

            foreach (ViewCallbacks initializer in viewListeners)
            {
                initializer.OnInitialize();
            }
        }

        [ContextMenu("Show")]
        public void Show()
        {
            Show(false, null);
        }

        public void Show(bool immediate)
        {
            Show(immediate, null);
        }

        public void Show(bool immediate, Action onComplete)
        {
            if (isShown)
                return;

            isShown = true;
            animationsCompleted = 0;
            gameObject.SetActive(true);
            Lock();
            
            foreach (ViewCallbacks initializer in viewListeners)
            {
                initializer.OnShow();
            }

            if (immediate)
            {
                OnShown();
                return;
            }

            if (toggleAnimations.Length > 0)
            {
                foreach (ViewAnimation toggleAnimation in toggleAnimations)
                {
                    toggleAnimation.Show(() =>
                    {
                        animationsCompleted++;

                        if (animationsCompleted == toggleAnimations.Length)
                        {
                            OnShown();
                        }
                    });
                }

                return;
            }

            OnShown();

            void OnShown()
            {
                Unlock();
                
                foreach (ViewCallbacks initializer in viewListeners)
                {
                    initializer.OnShown();
                }
                
                onShown?.Invoke();
                onComplete?.Invoke();
            }
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            Hide(false, null);
        }

        public void Hide(bool immediate)
        {
            Hide(immediate, null);
        }

        public void Hide(bool immediate, Action onComplete)
        {
            if (!isShown)
                return;

            isShown = false;
            animationsCompleted = 0;
            
            foreach (ViewCallbacks initializer in viewListeners)
            {
                initializer.OnHide();
            }

            if (immediate)
            {
                OnHidden();
                return;
            }

            if (toggleAnimations.Length > 0)
            {
                foreach (ViewAnimation toggleAnimation in toggleAnimations)
                {
                    toggleAnimation.Hide(() =>
                    {
                        animationsCompleted++;

                        if (animationsCompleted == toggleAnimations.Length)
                        {
                            OnHidden();
                        }
                    });
                }

                return;
            }

            OnHidden();

            void OnHidden()
            {
                onComplete?.Invoke();
                gameObject.SetActive(false);
                
                foreach (ViewCallbacks initializer in viewListeners)
                {
                    initializer.OnHidden();
                }

                animationsCompleted = 0;
                onHidden?.Invoke();
            }
        }

        public void Exit()
        {
            onExit?.Invoke();
        }

        public void Lock()
        {
            lockCount++;
            isLocked = lockCount > 0;
        }

        public void Unlock()
        {
            lockCount--;

            if (lockCount < 0)
            {
                lockCount = 0;
                Debug.LogWarning($"Lock count is {lockCount} for {name}! Resetting back to zero.", this);
            }

            isLocked = lockCount > 0;
        }

        private int lockCount;
    }
}