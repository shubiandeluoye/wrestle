using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace WJ.Core.Base.UI
{
    [RequireComponent(typeof(Button))]
    public class WJBaseUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected Button button;
        protected Image buttonImage;

        [Header("Button Settings")]
        [SerializeField] protected bool interactableOnStart = true;
        
        public UnityEvent onClick;
        public UnityEvent onPointerEnter;
        public UnityEvent onPointerExit;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            buttonImage = GetComponent<Image>();
        }

        protected virtual void Start()
        {
            button.onClick.AddListener(OnClick);
            SetInteractable(interactableOnStart);
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        protected virtual void OnClick()
        {
            onClick?.Invoke();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke();
        }

        public virtual void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }
    }
} 