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

        private static NetworkPrefabsRef instance;
        public static NetworkPrefabsRef Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<NetworkPrefabsRef>("NetworkPrefabsRef");
                return instance;
            }
        }
    }
} 