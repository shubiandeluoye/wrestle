using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.WJ.Core.Movement;

namespace Assets.Scripts.WJ.Core.Movement
{
    public class WJJoystickInput : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected RectTransform joystickBackground;  // 摇杆背景
        [SerializeField] protected RectTransform joystickHandle;      // 摇杆手柄
        [SerializeField] protected WJBaseMovement movement;           // 移动组件

        [Header("Settings")]
        [SerializeField] protected float moveRange = 50f;             // 摇杆移动范围
        
        protected bool isDragging;
        protected Vector2 moveInput;
        protected Vector2 joystickCenter;

        protected virtual void Start()
        {
            if (movement == null)
            {
                movement = GetComponent<WJBaseMovement>();
            }
            
            // 记录摇杆中心位置
            joystickCenter = joystickBackground.position;
        }

        public virtual void OnPointerDown(BaseEventData eventData)
        {
            isDragging = true;
            OnDrag(eventData);
        }

        public virtual void OnDrag(BaseEventData eventData)
        {
            if (eventData is PointerEventData pointerData)
            {
                Vector2 direction = pointerData.position - joystickCenter;
                moveInput = (direction.magnitude > moveRange) ? 
                    direction.normalized : 
                    direction / moveRange;

                // 更新摇杆手柄位置
                joystickHandle.position = joystickCenter + (moveInput * moveRange);
                
                // 发送移动输入
                movement.Move(moveInput);
            }
        }

        public virtual void OnPointerUp(BaseEventData eventData)
        {
            isDragging = false;
            moveInput = Vector2.zero;
            
            // 重置摇杆手柄位置
            joystickHandle.position = joystickCenter;
            
            // 停止移动
            movement.Move(Vector2.zero);
        }
    }
}
