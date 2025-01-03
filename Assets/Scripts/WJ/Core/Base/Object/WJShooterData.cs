using UnityEngine;

namespace Assets.Scripts.WJ.Core.Shooting.Base
{
    [CreateAssetMenu(fileName = "ShooterData", menuName = "WJ/ShooterData")]
    public class WJShooterData : ScriptableObject
    {
        [Header("Basic Settings")]
        public GameObject bulletPrefab;  // 子弹预制体
        public float fireRate = 0.5f;    // 射击间隔
        public float bulletSpeed = 6f;    // 子弹速度
        
        [Header("Advanced Settings")]
        public float spread = 0f;        // 射击散布
        public bool autoFire = false;    // 自动射击
        public int bulletsPerShot = 1;   // 每次射击的子弹数
        
        [Header("Bullet Settings")]
        public Vector3 bulletScale = new Vector3(5f, 5f, 5f);  // 添加子弹大小设置
    }
}