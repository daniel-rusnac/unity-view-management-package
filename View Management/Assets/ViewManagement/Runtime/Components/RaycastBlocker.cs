using UnityEngine;
using UnityEngine.UI;

namespace ViewManagement.Components
{
    public class RaycastBlocker
    {
        private Transform currentTarget;
        private RectTransform raycastBlockerRect;
        private Image raycastBlockerImage;
        
        public RaycastBlocker(Transform holder)
        {
            currentTarget = holder;
            
            GameObject raycastBlocker = new GameObject($"[Raycast Blocker] {currentTarget.name}");
            raycastBlockerImage = raycastBlocker.AddComponent<Image>();
            raycastBlockerImage.color = Color.clear;

            raycastBlockerRect = raycastBlocker.GetComponent<RectTransform>();
        }

        public void Block()
        {
            raycastBlockerImage.enabled = true;
        }

        public void Unblock()
        {
            raycastBlockerImage.enabled = false;
        }

        public void PlaceInFront(Transform target)
        {
            SetParent(target.parent);
        }

        public void PlaceInBack(Transform target)
        {
            SetParent(target.parent);
        }

        public void SetParent(Transform target)
        {
            currentTarget = target;
            raycastBlockerRect.SetParent(target);
            raycastBlockerRect.localScale = Vector3.one;
            
            raycastBlockerRect.anchorMin = Vector2.zero;
            raycastBlockerRect.anchorMax = Vector2.one;
            raycastBlockerRect.offsetMax = Vector2.zero;
            raycastBlockerRect.offsetMin = Vector2.zero;
        }
    }
}