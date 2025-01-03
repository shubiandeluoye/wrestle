using UnityEngine;
using Fusion;

namespace WJ.Core.Network.Base
{
    [CreateAssetMenu(fileName = "WJNetworkPrefabsRef", menuName = "WJ/Network/PrefabsRef")]
    public class WJNetworkPrefabsRef : ScriptableObject
    {
        [Header("Network Objects")]
        public NetworkPrefabRef playerPrefab;
        public NetworkPrefabRef scoreManagerPrefab;
        public NetworkPrefabRef bulletPrefab;

        [Header("Local Test Objects")]
        public GameObject localPlayerPrefab;
        public GameObject localScorePrefab;
        public GameObject localBulletPrefab;

        private static WJNetworkPrefabsRef instance;
        public static WJNetworkPrefabsRef Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<WJNetworkPrefabsRef>("WJNetworkPrefabsRef");
                    
                    if (instance == null)
                    {
                        Debug.LogWarning("WJNetworkPrefabsRef not found in Resources, creating new one...");
                        instance = CreateInstance<WJNetworkPrefabsRef>();
                    }
                }
                return instance;
            }
        }

        private void OnValidate()
        {
            if (localPlayerPrefab == null)
                Debug.LogWarning("Local Player Prefab is not set in WJNetworkPrefabsRef");
            if (localScorePrefab == null)
                Debug.LogWarning("Local Score Prefab is not set in WJNetworkPrefabsRef");
            if (localBulletPrefab == null)
                Debug.LogWarning("Local Bullet Prefab is not set in WJNetworkPrefabsRef");
        }
    }
} 