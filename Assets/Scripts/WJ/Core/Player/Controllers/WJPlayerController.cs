using UnityEngine;
using WJ.Core.Shooting.Base;
using WJ.Core.Input;
using UnityEngine.InputSystem;

namespace WJ.Core.Player.Controllers
{
    public class WJPlayerController : MonoBehaviour, WJInputActions.IPlayerActions
    {
        [Header("Shooting")]
        [SerializeField] private WJBaseShooter shooter;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float angleStep = 15f;

        [Header("Input")]
        private WJInputActions inputActions;
        
        // 状态变量
        private float currentAngle = 0f;

        private void Awake()
        {
            inputActions = new WJInputActions();
            inputActions.Player.SetCallbacks(this);
            inputActions.Player.Enable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            // 处理移动输入
        }

        public void OnStraightShoot(InputAction.CallbackContext context)
        {
            if (context.performed && shooter != null)
            {
                currentAngle = 0f;
                shooter.TryShoot(currentAngle);
            }
        }

        public void OnSwitchAngle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentAngle = (currentAngle + angleStep) % 360f;
                UpdateShootPointRotation();
            }
        }

        public void OnUpShoot(InputAction.CallbackContext context)
        {
            if (context.performed && shooter != null)
            {
                currentAngle += angleStep;
                shooter.TryShoot(currentAngle);
            }
        }

        public void OnDownShoot(InputAction.CallbackContext context)
        {
            if (context.performed && shooter != null)
            {
                currentAngle -= angleStep;
                shooter.TryShoot(currentAngle);
            }
        }

        public void OnSwitchBullet(InputAction.CallbackContext context)
        {
            // 处理切换子弹类型
        }

        private void UpdateShootPointRotation()
        {
            if (shootPoint != null)
            {
                shootPoint.rotation = Quaternion.Euler(0, 0, currentAngle);
                Debug.Log($"角度更新为: {currentAngle}");
            }
        }

        private void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Player.Disable();
                inputActions.Dispose();
            }
        }
    }
}