using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Fusion.Sockets;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Network
{
    public class WJNetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkRunner networkRunnerPrefab;
        [SerializeField] private NetworkInputHandler inputHandlerPrefab;
        [SerializeField] private bool useLocalMode = true;  // 添加本地模式开关
        
        private NetworkRunner runner;
        private NetworkInputHandler inputHandler;

        public async void StartGame(GameMode mode)
        {
            if (useLocalMode)
            {
                // 本地测试模式
                SpawnLocalPlayers();
                return;
            }

            // 确保有正确的配置
            if (networkRunnerPrefab == null)
            {
                Debug.LogError("Network Runner Prefab is not set!");
                return;
            }

            try
            {
                if (runner == null)
                    runner = Instantiate(networkRunnerPrefab);

                runner.AddCallbacks(this);

                var startGameArgs = new StartGameArgs()
                {
                    GameMode = mode,
                    SessionName = "WJGame",
                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                };

                Debug.Log("Starting Fusion connection...");
                await runner.StartGame(startGameArgs);
                Debug.Log("Fusion connection started!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error starting game: {e.Message}");
            }
        }

        private void SpawnLocalPlayers()
        {
            // 生成本地玩家1
            Vector3 leftSpawnPoint = new Vector3(-8, 0, 0);
            GameObject leftPlayer = Instantiate(
                NetworkPrefabsRef.Instance.localPlayerPrefab,
                leftSpawnPoint,
                Quaternion.identity
            );
            if (leftPlayer.TryGetComponent<WJPlayerController>(out var leftController))
            {
                leftController.SetPlayerId(1);  // 使用 SetPlayerId 而不是 SetPlayerSide
            }

            // 生成本地玩家2
            Vector3 rightSpawnPoint = new Vector3(8, 0, 0);
            GameObject rightPlayer = Instantiate(
                NetworkPrefabsRef.Instance.localPlayerPrefab,
                rightSpawnPoint,
                Quaternion.identity
            );
            if (rightPlayer.TryGetComponent<WJPlayerController>(out var rightController))
            {
                rightController.SetPlayerId(2);  // 使用 SetPlayerId 而不是 SetPlayerSide
            }

            // 生成计分板
            Instantiate(NetworkPrefabsRef.Instance.localScorePrefab);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                // 生成玩家
                Vector3 spawnPoint = GetSpawnPoint(player);
                var playerObject = runner.Spawn(NetworkPrefabsRef.Instance.playerPrefab, spawnPoint, Quaternion.identity, player);
                
                // 设置玩家ID
                if (playerObject.TryGetComponent<WJPlayerController>(out var controller))
                {
                    controller.SetPlayerId(player.PlayerId);
                }

                // 如果是第一个玩家，生成计分板
                if (player.PlayerId == 1)
                {
                    runner.Spawn(NetworkPrefabsRef.Instance.scoreManagerPrefab);
                }
            }
        }

        private Vector3 GetSpawnPoint(PlayerRef player)
        {
            return player.PlayerId == 1 ? new Vector3(-8, 0, 0) : new Vector3(8, 0, 0);
        }

        // 实现其他 INetworkRunnerCallbacks 接口方法...
        #region INetworkRunnerCallbacks
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        #endregion
    }
} 