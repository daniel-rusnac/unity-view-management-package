using UnityEngine;

namespace ViewManagement.Components
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupOnView : ViewCallbacks
    {
        private CanvasGroup canvasGroup;

        public override void OnInitialize()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void OnShown()
        {
            canvasGroup.interactable = true;
        }

        public override void OnHide()
        {
            canvasGroup.interactable = false;
        }
    }
}