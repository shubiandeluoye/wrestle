using Fusion;
using Fusion.Sockets;
using UnityEngine;
using WJ.Core.Input;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.WJ.Core.Network
{
    public class NetworkInputHandler : MonoBehaviour, INetworkRunnerCallbacks
    {
        private WJInputActions inputActions;
        private NetworkRunner runner;
        private NetworkInputData cachedInput;

        private void Awake()
        {
            inputActions = new WJInputActions();
            inputActions.Enable();
            cachedInput = new NetworkInputData
            {
                buttons = new NetworkButtons()
            };
        }

        public void Init(NetworkRunner runner)
        {
            this.runner = runner;
            runner.AddCallbacks(this);
        }

        private void Update()
        {
            if (runner == null || !runner.IsRunning) return;

            cachedInput.movement = inputActions.Player.Movement.ReadValue<Vector2>();
            
            cachedInput.buttons = new NetworkButtons();
            
            if (inputActions.Player.StraightShoot.triggered)
                cachedInput.buttons.Set(NetworkInputButtons.Shoot, true);
            if (inputActions.Player.UpShoot.triggered)
                cachedInput.buttons.Set(NetworkInputButtons.UpShoot, true);
            if (inputActions.Player.DownShoot.triggered)
                cachedInput.buttons.Set(NetworkInputButtons.DownShoot, true);
            if (inputActions.Player.SwitchAngle.triggered)
                cachedInput.buttons.Set(NetworkInputButtons.SwitchAngle, true);
            if (inputActions.Player.SwitchBullet.triggered)
                cachedInput.buttons.Set(NetworkInputButtons.SwitchBullet, true);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            input.Set(cachedInput);
        }

        private void OnDestroy()
        {
            if (runner != null)
                runner.RemoveCallbacks(this);
            
            inputActions?.Dispose();
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    }
} 