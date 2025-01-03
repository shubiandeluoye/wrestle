using UnityEngine;
using UnityEngine.Events;

namespace WJ.Core.Base.UI
{
    public class WJBaseUIPanel : WJBaseUI
    {
        [Header("Panel Settings")]
        [SerializeField] protected bool showOnStart = false;
        [SerializeField] protected bool destroyOnClose = false;

        public UnityEvent onShow;
        public UnityEvent onHide;

        protected virtual void Start()
        {
            if (showOnStart)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public override void Show()
        {
            base.Show();
            onShow?.Invoke();
        }

        public override void Hide()
        {
            base.Hide();
            onHide?.Invoke();

            if (destroyOnClose)
            {
                Destroy(gameObject);
            }
        }

        public virtual void Close()
        {
            Hide();
        }
    }
} 