using System;
using System.Collections.Generic;
using SOArchitecture.Channels;
using UnityEngine;

namespace ViewManagement
{
    [AddComponentMenu(ViewUtilities.ADD_MENU + nameof(ViewManager))]
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private InitializationMode initializationMode = InitializationMode.Start;
        [Tooltip("The back event for every UI. Escape button also works.")]
        [SerializeField] private VoidChannelSO backEventChannel;
        [Tooltip("The view that will be shown by default. Can be left empty.")]
        [SerializeField] private View startView;
        [SerializeField] private View[] views;

        public readonly List<View> activeViewsStack = new List<View>();

        private readonly Dictionary<View, Action> actionByView = new Dictionary<View, Action>();

        private void Awake()
        {
            if (initializationMode == InitializationMode.Awake)
                Initialize();
        }

        private void Start()
        {
            if (initializationMode == InitializationMode.Start)
                Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBack();
            }
        }

        private void OnEnable()
        {
            backEventChannel.Register(OnBack);
        }

        private void OnDisable()
        {
            backEventChannel.Unregister(OnBack);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < views.Length; i++)
            {
                if (actionByView.ContainsKey(views[i]))
                {
                    views[i].ShowEvent.Unregister((actionByView[views[i]]));
                }
            }
        }

        public void Initialize()
        {
            HashSet<VoidChannelSO> registeredChannels = new HashSet<VoidChannelSO>();
            registeredChannels.Add(backEventChannel);

            foreach (View view in views)
            {
                if (registeredChannels.Contains(view.ShowEvent))
                {
                    Debug.LogWarning($"The channel [{view.ShowEvent.name}] is registered for more than one view!",
                        view.ShowEvent);
                }
                else
                {
                    registeredChannels.Add(view.ShowEvent);
                }

                if (actionByView.ContainsKey(view))
                {
                    Debug.LogWarning($"View [{view.name}] is initialized for the secondTime!", view);
                    continue;
                }

                view.Initialize();
                view.Hide(true);

                actionByView.Add(view, () => ShowView(view));
                view.ShowEvent.Register(actionByView[view]);
            }

            if (startView != null)
            {
                ShowView(startView);
            }
        }

        private void ShowView(View view)
        {
            while (activeViewsStack.Count > 0)
            {
                View lastView = activeViewsStack[activeViewsStack.Count - 1];

                if (lastView == view)
                {
                    OnShow(view);
                    return;
                }

                if (lastView.Depth < view.Depth)
                {
                    if (view.Mode == ViewMode.Swap)
                    {
                        OnHide(lastView);
                    }
                    else
                    {
                        OnShow(lastView);
                    }

                    break;
                }

                OnHide(lastView);
                activeViewsStack.RemoveAt(activeViewsStack.Count - 1);
            }

            if (activeViewsStack.Count > 0 && view.Mode == ViewMode.Swap)
            {
                foreach (View activeView in activeViewsStack)
                {
                    activeView.Hide();
                }
            }

            activeViewsStack.Add(view);
            OnShow(view);
        }

        private void OnBack()
        {
            if (activeViewsStack.Count == 0)
                return;

            if (activeViewsStack.Count == 1)
            {
                activeViewsStack[activeViewsStack.Count - 1].Exit();
                return;
            }

            View lastView = activeViewsStack[activeViewsStack.Count - 1];
            activeViewsStack.Remove(lastView);
            OnHide(lastView);
            
            ShowView(activeViewsStack[activeViewsStack.Count - 1]);

            for (int i = activeViewsStack.Count - 1; i > 0; i--)
            {
                if (activeViewsStack[i].Mode == ViewMode.Overlay)
                {
                    activeViewsStack[i - 1].Show();
                }
            }
        }

        private void OnShow(View view, int siblingOffset = 1)
        {
            int viewSiblingIndex = view.transform.parent.childCount - siblingOffset;
            view.transform.SetSiblingIndex(viewSiblingIndex);
            view.Show();
        }

        private void OnHide(View view)
        {
            view.Hide();
        }

        public void LoadViews()
        {
            views = FindObjectsOfType<View>(true);
        }

        public void ClearViews()
        {
            views = new View[0];
        }
    }
}