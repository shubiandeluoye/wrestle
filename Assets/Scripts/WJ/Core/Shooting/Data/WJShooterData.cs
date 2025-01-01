using UnityEngine;

namespace WJ.Core.Shooting.Data
{
    [CreateAssetMenu(fileName = "WJShooterData", menuName = "WJ/Shooting/ShooterData")]
    public class WJShooterData : ScriptableObject
    {
        [Header("Basic Settings")]
        public float fireRate = 0.5f;        // 射击频率
        public float bulletSpeed = 20f;       // 子弹速度
        public float bulletDamage = 10f;      // 子弹伤害
        public float bulletLifetime = 2f;     // 子弹存活时间

        [Header("Advanced Settings")]
        public float spread = 0f;             // 射击散布
        public int bulletsPerShot = 1;        // 每次射击的子弹数
        public bool autoFire = false;         // 是否自动射击
    }
}
