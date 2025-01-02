using UnityEngine;
using Assets.Scripts.WJ.Core.Shooting.Base;
using WJ.Core.Input;
using Assets.Scripts.WJ.Core.Movement;
using UnityEngine.InputSystem;
using Assets.Scripts.WJ.Core.Game;

namespace Assets.Scripts.WJ.Core.Player.Controllers
{
    public class WJPlayerController : MonoBehaviour, WJInputActions.IPlayerActions
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
        
        private bool isAngle30 = true;
        private WJInputActions inputActions;

        [Header("Bounds Check")]
        [SerializeField] private float minY = -10f;

        private int playerId;

        private void Awake()
        {
            inputActions = new WJInputActions();
            inputActions.Player.SetCallbacks(this);
            inputActions.Enable();
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
            if (context.performed && shooter != null)
            {
                shooter.TryShoot(0f);
            }
        }

        public void OnUpShoot(InputAction.CallbackContext context)
        {
            if (context.performed && shooter != null)
            {
                float currentAngle = isAngle30 ? angle30 : angle45;
                shooter.TryShoot(currentAngle);
            }
        }

        public void OnDownShoot(InputAction.CallbackContext context)
        {
            if (context.performed && shooter != null)
            {
                float currentAngle = isAngle30 ? angle30 : angle45;
                shooter.TryShoot(-currentAngle);
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
                var scoreManager = FindObjectOfType<WJScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.PlayerOutOfBounds(this);
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

        public bool GetIsLeftPlayer()
        {
            return playerId == 1 || playerId == 3;
        }

        public void SetPlayerSide(bool isLeft)
        {
            isLeftPlayer = isLeft;
        }

        public void SetPlayerId(int id)
        {
            playerId = id;
        }

        public int GetTeamId()
        {
            return (playerId % 2 == 1) ? 1 : 2;
        }

        public int GetPlayerId()
        {
            return playerId;
        }
    }
}