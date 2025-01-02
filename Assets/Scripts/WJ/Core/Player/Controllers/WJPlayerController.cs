using UnityEngine;
using Assets.Scripts.WJ.Core.Shooting.Base;
using WJ.Core.Input;
using Assets.Scripts.WJ.Core.Movement;
using UnityEngine.InputSystem;
using Assets.Scripts.WJ.Core.Game;
using Fusion;
using Assets.Scripts.WJ.Core.Network;

namespace Assets.Scripts.WJ.Core.Player.Controllers
{
    public class WJPlayerController : NetworkBehaviour, WJInputActions.IPlayerActions
    {
        [Header("Components")]
        [SerializeField] private WJBaseMovement movement;
        [SerializeField] private WJBaseShooter shooter;
        [SerializeField] private Transform shootPoint;
        
        [Header("Player Settings")]
        [SerializeField] private bool isLeftPlayer;
        
        [Header("Shooting Settings")]
        [SerializeField] private float angle30 = 30f;
        [SerializeField] private float angle45 = 45f;
        
        private bool isAngle30 = true;  // true = 30度, false = 45度

        private WJInputActions inputActions;

        public bool IsLeftPlayer { get; private set; }

        [Header("Bounds Check")]
        [SerializeField] private float minY = -10f;  // Y轴最低点

        [Networked]
        private NetworkButtons ButtonsPrevious { get; set; }

        private void Awake()
        {
            inputActions = new WJInputActions();
            inputActions.Player.SetCallbacks(this);
            inputActions.Player.Enable();
            IsLeftPlayer = isLeftPlayer;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (movement != null)
            {
                Vector2 input = context.ReadValue<Vector2>();
                movement.Move(input);
            }
        }

        public void OnStraightShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("[PlayerController] Straight shoot input received");
                if (shooter != null)
                {
                    shooter.TryShoot(0f);
                }
                else
                {
                    Debug.LogError("[PlayerController] Shooter component is null!");
                }
            }
        }

        public void OnUpShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("[PlayerController] Up shoot input received");
                if (shooter != null)
                {
                    float currentAngle = isAngle30 ? angle30 : angle45;
                    shooter.TryShoot(currentAngle);
                }
                else
                {
                    Debug.LogError("[PlayerController] Shooter component is null!");
                }
            }
        }

        public void OnDownShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("[PlayerController] Down shoot input received");
                if (shooter != null)
                {
                    float currentAngle = isAngle30 ? angle30 : angle45;
                    shooter.TryShoot(-currentAngle);
                }
                else
                {
                    Debug.LogError("[PlayerController] Shooter component is null!");
                }
            }
        }

        public void OnSwitchAngle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isAngle30 = !isAngle30;
                string currentAngle = isAngle30 ? "30" : "45";
                Debug.Log($"Switched to {currentAngle} degrees");
            }
        }

        public void OnSwitchBullet(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Switching bullet type");
            }
        }

        private void Update()
        {
            // 检查是否掉出地图
            if (transform.position.y < minY)
            {
                var scoreManager = FindObjectOfType<WJNetworkScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.PlayerOutOfBounds(IsLeftPlayer);
                }
            }
        }

        private void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions.Dispose();
            }
        }

        private void TryShoot(float angle)
        {
            if (shooter != null)
            {
                shooter.TryShoot(angle);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                // 处理移动
                if (movement != null)
                    movement.Move(data.movement);

                // 处理射击
                var pressed = data.buttons.GetPressed(ButtonsPrevious);
                if (pressed.IsSet(NetworkInputButtons.Shoot))
                    TryShoot(0);
                if (pressed.IsSet(NetworkInputButtons.UpShoot))
                    TryShoot(isAngle30 ? angle30 : angle45);
                if (pressed.IsSet(NetworkInputButtons.DownShoot))
                    TryShoot(isAngle30 ? -angle30 : -angle45);

                ButtonsPrevious = data.buttons;
            }
        }
    }
}