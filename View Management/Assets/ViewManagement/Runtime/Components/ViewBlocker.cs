using UnityEngine;
using UnityEngine.UI;

namespace ViewManagement.Components
{
    /// <summary>
    /// Blocks the view during animations.
    /// </summary>
    [AddComponentMenu(ViewUtilities.COMPONENTS_SUB_MENU + nameof(ViewBlocker))]
    public class ViewBlocker : ViewCallbacks
    {
        private RectTransform raycastBlockerRect;
        private Image raycastBlockerImage;

        public override void OnInitialize()
        {
            GameObject raycastBlocker = new GameObject($"[Raycast Blocker] {name}");
            raycastBlockerImage = raycastBlocker.AddComponent<Image>();
            raycastBlockerImage.color = Color.clear;

            raycastBlockerRect = raycastBlocker.GetComponent<RectTransform>();

            raycastBlockerRect.SetParent(transform);
            raycastBlockerRect.localScale = Vector3.one;

            raycastBlockerRect.anchorMin = Vector2.zero;
            raycastBlockerRect.anchorMax = Vector2.one;
            raycastBlockerRect.offsetMax = Vector2.zero;
            raycastBlockerRect.offsetMin = Vector2.zero;
        }

        public void Block()
        {
            raycastBlockerRect.SetAsLastSibling();
        }

        public void Unblock()
        {
            raycastBlockerRect.SetAsFirstSibling();
        }

        public override void OnShow()
        {
            Block();
        }

        public override void OnShown()
        {
            Unblock();
        }

        public override void OnHide()
        {
            Block();
        }
    }
}