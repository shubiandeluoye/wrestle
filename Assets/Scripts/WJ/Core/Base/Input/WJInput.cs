using UnityEngine;
using UnityEngine.InputSystem;

namespace WJ.Core.Base.Input
{
    public class WJInput : MonoBehaviour, WJInputInterface
    {
        protected WJInputActions inputActions;
        protected Vector2 moveInput;

        protected virtual void Awake()
        {
            inputActions = new WJInputActions();
            inputActions.Player.Enable();
            inputActions.Player.Movement.performed += OnMovementPerformed;
            inputActions.Player.Movement.canceled += OnMovementCanceled;
        }

        protected virtual void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Player.Movement.performed -= OnMovementPerformed;
                inputActions.Player.Movement.canceled -= OnMovementCanceled;
                inputActions.Player.Disable();
                inputActions.Dispose();
            }
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            moveInput = Vector2.zero;
        }

        public virtual Vector2 GetMovementInput()
        {
            return moveInput;
        }

        public virtual bool GetButtonDown(string buttonName)
        {
            var action = inputActions.asset.FindAction(buttonName);
            return action?.triggered ?? false;
        }

        public virtual bool GetButton(string buttonName)
        {
            var action = inputActions.asset.FindAction(buttonName);
            return action?.IsPressed() ?? false;
        }

        public virtual bool GetButtonUp(string buttonName)
        {
            var action = inputActions.asset.FindAction(buttonName);
            return action?.WasReleasedThisFrame() ?? false;
        }
    }
} 