using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Fusion.Sockets;

namespace Assets.Scripts.WJ.Core.Network
{
    public class WJNetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkRunner networkRunnerPrefab;
        [SerializeField] private NetworkInputHandler inputHandlerPrefab;
        
        private NetworkRunner runner;
        private NetworkInputHandler inputHandler;

        public static event Action<NetworkRunner> OnJoinedGame;

        public async void StartGame(GameMode mode)
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

            await runner.StartGame(startGameArgs);
            
            // 创建输入处理器
            if (inputHandler == null)
            {
                inputHandler = Instantiate(inputHandlerPrefab);
                inputHandler.Init(runner);
            }

            OnJoinedGame?.Invoke(runner);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                // 生成玩家
                Vector3 spawnPoint = GetSpawnPoint(player);
                runner.Spawn(NetworkPrefabsRef.Instance.playerPrefab, spawnPoint, Quaternion.identity, player);

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