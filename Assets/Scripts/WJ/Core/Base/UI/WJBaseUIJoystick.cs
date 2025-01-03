using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WJ.Core.Base.UI
{
    public class WJBaseUIJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("Joystick References")]
        [SerializeField] protected RectTransform background;
        [SerializeField] protected RectTransform handle;

        [Header("Joystick Settings")]
        [SerializeField] protected float moveThreshold = 1f;
        [SerializeField] protected float joystickRange = 50f;
        [SerializeField] protected bool hideOnRelease = true;

        protected Vector2 input = Vector2.zero;
        protected Vector2 center;
        protected bool isDragging = false;

        public Vector2 Input => input;
        public bool IsDragging => isDragging;

        protected virtual void Awake()
        {
            if (hideOnRelease)
            {
                Hide();
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            Show();
            
            // 设置摇杆位置
            background.position = eventData.position;
            handle.position = eventData.position;
            center = background.position;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            Vector2 direction = eventData.position - center;
            input = Vector2.ClampMagnitude(direction / joystickRange, 1f);

            // 只有超过阈值才移动
            if (direction.magnitude > moveThreshold)
            {
                handle.position = center + input * joystickRange;
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            input = Vector2.zero;
            handle.position = center;

            if (hideOnRelease)
            {
                Hide();
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Reset()
        {
            isDragging = false;
            input = Vector2.zero;
            handle.position = center;
        }
    }
} 