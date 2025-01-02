using Fusion;
using Fusion.Sockets;
using UnityEngine;
using WJ.Core.Input;

namespace Assets.Scripts.WJ.Core.Network
{
    public class NetworkInputHandler : MonoBehaviour
    {
        private WJInputActions inputActions;
        private NetworkRunner runner;

        private void Awake()
        {
            inputActions = new WJInputActions();
            inputActions.Enable();
        }

        public void Init(NetworkRunner runner)
        {
            this.runner = runner;
        }

        private void Update()
        {
            if (runner == null || !runner.IsRunning) return;

            var data = new NetworkInputData
            {
                movement = inputActions.Player.Movement.ReadValue<Vector2>(),
                buttons = new NetworkButtons()
            };
            
            // 读取按钮输入
            if (inputActions.Player.StraightShoot.triggered)
                data.buttons.Set(NetworkInputButtons.Shoot, true);
            if (inputActions.Player.UpShoot.triggered)
                data.buttons.Set(NetworkInputButtons.UpShoot, true);
            if (inputActions.Player.DownShoot.triggered)
                data.buttons.Set(NetworkInputButtons.DownShoot, true);
            if (inputActions.Player.SwitchAngle.triggered)
                data.buttons.Set(NetworkInputButtons.SwitchAngle, true);
            if (inputActions.Player.SwitchBullet.triggered)
                data.buttons.Set(NetworkInputButtons.SwitchBullet, true);

            // 提交输入
            runner.SetPlayerInput(runner.LocalPlayer, data);
        }

        private void OnDestroy()
        {
            inputActions?.Dispose();
        }
    }
} 