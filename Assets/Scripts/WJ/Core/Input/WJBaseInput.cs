using UnityEngine;
using UnityEngine.InputSystem;

namespace WJ.Core.Input
{
    public class WJBaseInput : MonoBehaviour
    {
        protected Vector2 moveInput;
        protected WJInputActions inputActions;

        protected virtual void Awake()
        {
            // 初始化输入系统
            inputActions = new WJInputActions();
            inputActions.Player.Enable();

            // 订阅移动输入事件
            inputActions.Player.Movement.performed += OnMovementPerformed;
            inputActions.Player.Movement.canceled += OnMovementCanceled;
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            moveInput = Vector2.zero;
        }

        protected virtual void OnDestroy()
        {
            // 取消订阅事件
            if (inputActions != null)
            {
                inputActions.Player.Movement.performed -= OnMovementPerformed;
                inputActions.Player.Movement.canceled -= OnMovementCanceled;
                inputActions.Player.Disable();
                inputActions.Dispose();
            }
        }

        // 获取移动输入
        public Vector2 GetMovementInput()
        {
            return moveInput;
        }
    }
}
