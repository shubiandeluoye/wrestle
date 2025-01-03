using UnityEngine;
using UnityEngine.Events;

namespace WJ.Core.Base.UI
{
    public class WJBaseUIPopup : WJBaseUIPanel
    {
        [Header("Popup Settings")]
        [SerializeField] protected bool closeOnBackdropClick = true;
        [SerializeField] protected WJBaseUIButton confirmButton;
        [SerializeField] protected WJBaseUIButton cancelButton;
        [SerializeField] protected WJBaseUIText titleText;
        [SerializeField] protected WJBaseUIText contentText;

        public UnityEvent onConfirm;
        public UnityEvent onCancel;

        protected override void Start()
        {
            base.Start();
            
            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirm);
                
            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancel);
        }

        protected virtual void OnConfirm()
        {
            onConfirm?.Invoke();
            Close();
        }

        protected virtual void OnCancel()
        {
            onCancel?.Invoke();
            Close();
        }

        public virtual void SetTitle(string title)
        {
            if (titleText != null)
                titleText.SetText(title);
        }

        public virtual void SetContent(string content)
        {
            if (contentText != null)
                contentText.SetText(content);
        }

        public virtual void Setup(string title, string content, UnityAction onConfirmAction = null, UnityAction onCancelAction = null)
        {
            SetTitle(title);
            SetContent(content);

            if (onConfirmAction != null)
                onConfirm.AddListener(onConfirmAction);
                
            if (onCancelAction != null)
                onCancel.AddListener(onCancelAction);
        }

        protected override void OnDestroy()
        {
            if (confirmButton != null)
                confirmButton.onClick.RemoveListener(OnConfirm);
                
            if (cancelButton != null)
                cancelButton.onClick.RemoveListener(OnCancel);

            onConfirm.RemoveAllListeners();
            onCancel.RemoveAllListeners();
        }
    }
} 