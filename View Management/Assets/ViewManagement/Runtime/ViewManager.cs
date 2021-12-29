using System;
using System.Collections.Generic;
using SOArchitecture.Channels;
using UnityEngine;
using UnityEngine.UI;

namespace ViewManagement
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private VoidChannelSO backEventChannel;
        [SerializeField] private View startView;
        [SerializeField] private View[] views;

        public readonly Stack<View> activeViewsStack = new Stack<View>();
        private readonly Dictionary<View, Action> actionByView = new Dictionary<View, Action>();
        private RectTransform raycastBlockerRect;

        private void Start()
        {
            InitializeRaycastBlocker();
            InitializeViews();
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

        private void InitializeRaycastBlocker()
        {
            if (views.Length == 0)
                return;

            GameObject raycastBlocker = new GameObject($"{name} Raycast Blocker");
            Image image = raycastBlocker.AddComponent<Image>();
            image.color = Color.clear;

            raycastBlockerRect = raycastBlocker.GetComponent<RectTransform>();
            
            SetRaycastBlockerSibling(0, views[0].transform.parent);
        }

        private void InitializeViews()
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

                actionByView.Add(view, () => OnShow(view));
                view.ShowEvent.Register(actionByView[view]);
            }

            if (startView != null)
            {
                OnShow(startView);
            }
        }

        private void OnShow(View view)
        {
            while (activeViewsStack.Count > 0)
            {
                View lastView = activeViewsStack.Peek();

                if (lastView == view)
                {
                    ShowView(view);
                    return;
                }

                if (lastView.Depth < view.Depth)
                {
                    if (view.Mode == ViewMode.Swap)
                    {
                        HideView(lastView);
                    }
                    else
                    {
                        ShowView(lastView);
                    }

                    break;
                }

                HideView(activeViewsStack.Pop());
            }

            activeViewsStack.Push(view);
            ShowView(view);
        }

        private void OnBack()
        {
            if (activeViewsStack.Count > 1)
            {
                if (activeViewsStack.Peek().IsLocked)
                    return;

                View lastView = activeViewsStack.Pop();

                if (activeViewsStack.Peek().Depth < lastView.Depth)
                {
                    HideView(lastView);
                    ShowView(activeViewsStack.Peek(), 2);
                    return;
                }

                activeViewsStack.Push(lastView);
            }

            activeViewsStack.Peek().Exit();
        }

        private void ShowView(View view, int siblingOffset = 1)
        {
            if (view.IsShown)
                return;

            int viewSiblingIndex = view.transform.parent.childCount - siblingOffset;
            
            view.transform.SetSiblingIndex(viewSiblingIndex);
           
            SetRaycastBlockerSibling(viewSiblingIndex + 1, view.transform.parent);
            view.Show(false, () => SetRaycastBlockerSibling(viewSiblingIndex - 1, view.transform.parent));
        }

        private void SetRaycastBlockerSibling(int siblingIndex, Transform parent)
        {
            raycastBlockerRect.SetParent(parent);
            raycastBlockerRect.localScale = Vector3.one;
            raycastBlockerRect.SetSiblingIndex(siblingIndex);
            
            raycastBlockerRect.anchorMin = Vector2.zero;
            raycastBlockerRect.anchorMax = Vector2.one;
            raycastBlockerRect.offsetMax = Vector2.zero;
            raycastBlockerRect.offsetMin = Vector2.zero;
        }

        private void HideView(View view)
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