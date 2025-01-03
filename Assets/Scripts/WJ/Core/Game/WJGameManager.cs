using UnityEngine;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Game
{
    public class WJGameManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject scoreManagerPrefab;

        [Header("Spawn Settings - Left Player")]
        [SerializeField] private Vector3 leftPlayerPosition = new Vector3(-90f, 80f, 0f);
        [SerializeField] private Vector3 leftPlayerScale = new Vector3(12f, 12f, 12f);

        [Header("Spawn Settings - Right Player")]
        [SerializeField] private Vector3 rightPlayerPosition = new Vector3(90f, 80f, 0f);
        [SerializeField] private Vector3 rightPlayerScale = new Vector3(12f, 12f, 12f);

        private void Start()
        {
            SpawnPlayers();
        }

        private void SpawnPlayers()
        {
            Debug.Log("Spawning players...");

            // 生成左边玩家
            GameObject leftPlayer = Instantiate(playerPrefab, leftPlayerPosition, Quaternion.identity);
            leftPlayer.transform.localScale = leftPlayerScale;
            if (leftPlayer.TryGetComponent<WJPlayerController>(out var leftController))
            {
                leftController.SetPlayerId(1);  // 设置ID时会自动设置朝向
            }

            // 生成右边玩家
            GameObject rightPlayer = Instantiate(playerPrefab, rightPlayerPosition, Quaternion.identity);
            rightPlayer.transform.localScale = rightPlayerScale;
            if (rightPlayer.TryGetComponent<WJPlayerController>(out var rightController))
            {
                rightController.SetPlayerId(2);  // 设置ID时会自动设置朝向
            }

            // 生成计分板
            Instantiate(scoreManagerPrefab);

            Debug.Log($"Left player spawned at: {leftPlayerPosition}");
            Debug.Log($"Right player spawned at: {rightPlayerPosition}");
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // 增大显示尺寸
            float gizmosRadius = 10f;  // 增大半径
            
            // 在场景视图中显示生成点
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(leftPlayerPosition, gizmosRadius);
            // 添加方向指示
            Gizmos.DrawLine(leftPlayerPosition, leftPlayerPosition + Vector3.up * 20f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rightPlayerPosition, gizmosRadius);
            Gizmos.DrawLine(rightPlayerPosition, rightPlayerPosition + Vector3.up * 20f);

            // 添加文字标签
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(leftPlayerPosition + Vector3.up * 25f, "Left Player Spawn");
            UnityEditor.Handles.Label(rightPlayerPosition + Vector3.up * 25f, "Right Player Spawn");
        }
#endif
    }
} 