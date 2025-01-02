using UnityEngine;
using Fusion;

namespace Assets.Scripts.WJ.Core.Game
{
    public class WJGameManager : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef scoreManagerPrefab;
        
        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                // 在网络中生成计分管理器
                Runner.Spawn(scoreManagerPrefab);
            }
        }
    }
} 