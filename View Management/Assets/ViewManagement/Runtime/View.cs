using System;
using SOArchitecture.Channels;
using UnityEngine;
using ViewManagement.Components;

namespace ViewManagement
{
    [DisallowMultipleComponent]
    public class View : MonoBehaviour
    {
        private const ViewSetting DEFAULT_SETTINGS = (ViewSetting) (-1);

        [SerializeField] private int depth;
        [SerializeField] private VoidChannelSO showEvent;
        [SerializeField] private ViewSetting settings = DEFAULT_SETTINGS;
        [SerializeField] private ViewMode mode;
        [HideInInspector] public ViewCallbacksController viewCallbacksController = new ViewCallbacksController();

        private int lockCount;
        private int animationsCompleted;
        private bool isLocked;
        private ViewAnimation[] toggleAnimations;

        internal ViewSetting Settings => settings;

        public event Action onShown;
        public event Action onHidden;
        public int Depth => depth;
        public VoidChannelSO ShowEvent => showEvent;
        public ViewMode Mode => mode;
        public ViewState State { get; private set; }

        [Obsolete("Use [State.IsShown] instead.")]
        public bool IsShown => State == ViewState.IsShown;

        public bool IsLocked => isLocked;

        public void Initialize()
        {
            toggleAnimations = GetComponents<ViewAnimation>();
            viewCallbacksController.callbacks = GetComponents<ViewCallbacks>();
            
            SetState(gameObject.activeSelf ? ViewState.IsShown : ViewState.IsHidden);
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
            if (State == ViewState.IsShown || State == ViewState.IsShowing)
                return;

            SetState(ViewState.IsShowing);
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
                SetState(ViewState.IsShown);
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
            if (State == ViewState.IsHidden || State == ViewState.IsHiding)
                return;

            SetState(ViewState.IsHiding);
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
                if (State == ViewState.IsShowing)
                {
                    Unlock();
                }

                SetState(ViewState.IsHidden);

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

        private void SetState(ViewState state)
        {
            if (State == state)
                return;

            State = state;
        }
    }
}