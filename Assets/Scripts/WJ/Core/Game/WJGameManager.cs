using UnityEngine;
using Assets.Scripts.WJ.Core.Audio;

namespace Assets.Scripts.WJ.Core.Game
{
    public class WJGameManager : MonoBehaviour
    {
        public static WJGameManager Instance { get; private set; }

        [Header("Manager References")]
        [SerializeField] private WJAudioManager audioManager;

        [Header("Player Settings")]
        [SerializeField] private GameObject playerPrefab;  // 玩家预制体
        [SerializeField] private Transform player1SpawnPoint;  // 玩家1出生点
        [SerializeField] private Transform player2SpawnPoint;  // 玩家2出生点

        [Header("Game Mode")]
        [SerializeField] private bool isSinglePlayer = true;  // 添加单人模式开关

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                if (audioManager == null)
                    audioManager = GetComponentInChildren<WJAudioManager>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SpawnPlayers();
        }

        private void SpawnPlayers()
        {
            if (playerPrefab != null)
            {
                if (player1SpawnPoint != null)
                {
                    // 生成玩家1
                    GameObject player1 = Instantiate(playerPrefab, player1SpawnPoint.position, player1SpawnPoint.rotation);
                    player1.name = "Player1";
                }
                
                // 只在双人模式下生成玩家2
                if (!isSinglePlayer && player2SpawnPoint != null)
                {
                    GameObject player2 = Instantiate(playerPrefab, player2SpawnPoint.position, player2SpawnPoint.rotation);
                    player2.name = "Player2";
                }
            }
            else
            {
                Debug.LogError("Player prefab is not assigned!");
            }
        }

        // 提供访问器
        public WJAudioManager AudioManager => audioManager;
    }
} 