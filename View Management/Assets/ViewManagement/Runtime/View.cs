using System;
using SOArchitecture.Channels;
using UnityEngine;
using ViewManagement.Components;

namespace ViewManagement
{
    [AddComponentMenu(ViewUtilities.ADD_MENU + nameof(View))]
    [DisallowMultipleComponent]
    public class View : MonoBehaviour
    {
        [Tooltip("The depth of the view in stack. Higher depth means that it will remember the previous view and will show it when pressing back.")]
        [SerializeField] private int depth;
        [Tooltip("The event to be fired for this view to be shown.")]
        [SerializeField] private VoidChannelSO showEvent;
        [Tooltip("How should the view be drawn relative to the last active view.")]
        [SerializeField] private ViewMode mode;
        [HideInInspector] public ViewCallbacksController viewCallbacksController = new ViewCallbacksController();
        
        private int animationsCompleted;
        private ViewAnimation[] toggleAnimations;

        public int Depth => depth;
        public VoidChannelSO ShowEvent => showEvent;
        public ViewMode Mode => mode;
        public ViewState State { get; private set; }

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

                viewCallbacksController.OnShown();

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
                SetState(ViewState.IsHidden);

                onComplete?.Invoke();
                gameObject.SetActive(false);

                viewCallbacksController.OnHidden();

                animationsCompleted = 0;
            }
        }

        public void Exit()
        {
            viewCallbacksController.OnExit();
        }

        private void SetState(ViewState state)
        {
            if (State == state)
                return;

            State = state;
        }
    }
}