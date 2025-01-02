using UnityEngine;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Game
{
    public class WJGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject scoreManagerPrefab;

        private void Start()
        {
            SpawnPlayers();
        }

        private void SpawnPlayers()
        {
            // 生成左边玩家
            Vector3 leftSpawnPoint = new Vector3(-8, 0, 0);
            GameObject leftPlayer = Instantiate(playerPrefab, leftSpawnPoint, Quaternion.identity);
            if (leftPlayer.TryGetComponent<WJPlayerController>(out var leftController))
            {
                leftController.SetPlayerId(1);  // ID 1 = 左边玩家
            }

            // 生成右边玩家
            Vector3 rightSpawnPoint = new Vector3(8, 0, 0);
            GameObject rightPlayer = Instantiate(playerPrefab, rightSpawnPoint, Quaternion.identity);
            if (rightPlayer.TryGetComponent<WJPlayerController>(out var rightController))
            {
                rightController.SetPlayerId(2);  // ID 2 = 右边玩家
            }

            // 生成计分板
            Instantiate(scoreManagerPrefab);
        }
    }
} 