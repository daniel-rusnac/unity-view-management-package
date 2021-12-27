using UnityEngine;

namespace ViewManagement
{
    public enum ViewMode
    {
        [Tooltip("Hide previous view and show this one.")]
        Swap,
        [Tooltip("Draw this view on top.")]
        Overlay
    }
}