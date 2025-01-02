using UnityEngine;
using Fusion;

namespace Assets.Scripts.WJ.Core.Network
{
    [CreateAssetMenu(fileName = "NetworkPrefabsRef", menuName = "WJ/Network/PrefabsRef")]
    public class NetworkPrefabsRef : ScriptableObject
    {
        [Header("Network Objects")]
        public NetworkPrefabRef playerPrefab;
        public NetworkPrefabRef scoreManagerPrefab;
        public NetworkPrefabRef bulletPrefab;

        [Header("Local Test Objects")]
        public GameObject localPlayerPrefab;
        public GameObject localScorePrefab;
        public GameObject localBulletPrefab;

        private static NetworkPrefabsRef instance;
        public static NetworkPrefabsRef Instance
        {
            get
            {
                if (instance == null)
                {
                    // 尝试从 Resources 加载
                    instance = Resources.Load<NetworkPrefabsRef>("NetworkPrefabsRef");
                    
                    // 如果还是空，创建一个新的
                    if (instance == null)
                    {
                        Debug.LogWarning("NetworkPrefabsRef not found in Resources, creating new one...");
                        instance = CreateInstance<NetworkPrefabsRef>();
                    }
                }
                return instance;
            }
        }

        // 添加验证方法
        private void OnValidate()
        {
            if (localPlayerPrefab == null)
                Debug.LogWarning("Local Player Prefab is not set in NetworkPrefabsRef");
            if (localScorePrefab == null)
                Debug.LogWarning("Local Score Prefab is not set in NetworkPrefabsRef");
            if (localBulletPrefab == null)
                Debug.LogWarning("Local Bullet Prefab is not set in NetworkPrefabsRef");
        }
    }
} 