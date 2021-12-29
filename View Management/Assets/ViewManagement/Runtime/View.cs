using System;
using SOArchitecture.Channels;
using UnityEngine;
using ViewManagement.Components;

namespace ViewManagement
{
    public class View : MonoBehaviour
    {
        [SerializeField] private int depth;
        [SerializeField] private VoidChannelSO showEvent;
        [SerializeField] private ViewMode mode;
        [HideInInspector] public ViewCallbacksController viewCallbacksController = new ViewCallbacksController();

        private int lockCount;
        private int animationsCompleted;
        private bool isShown;
        private bool isLocked;
        private bool isShowAnimation;
        private ViewAnimation[] toggleAnimations;

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
            toggleAnimations = GetComponents<ViewAnimation>();
            viewCallbacksController.callbacks = GetComponents<ViewCallbacks>();
            viewCallbacksController.OnInitialize();
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

            isShowAnimation = true;
            isShown = true;
            animationsCompleted = 0;
            gameObject.SetActive(true);
            Lock();

            viewCallbacksController.OnShow();

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
                isShowAnimation = false;
                Unlock();

                viewCallbacksController.OnShown();

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

            viewCallbacksController.OnHide();

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
                if (isShowAnimation)
                {
                    isShowAnimation = false;
                    Unlock();
                }

                onComplete?.Invoke();
                gameObject.SetActive(false);

                viewCallbacksController.OnHidden();

                animationsCompleted = 0;
                onHidden?.Invoke();
            }
        }

        public void Exit()
        {
            viewCallbacksController.OnExit();
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
    }
}