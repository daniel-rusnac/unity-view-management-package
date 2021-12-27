using UnityEngine;

namespace ViewManagement
{
    public abstract class ViewCallbacks : MonoBehaviour
    {
        public virtual void OnInitialize()
        {
        }

        public virtual void OnShow()
        {
        }

        public virtual void OnShown()
        {
        }

        public virtual void OnHide()
        {
        }

        public virtual void OnHidden()
        {
        }
    }
}