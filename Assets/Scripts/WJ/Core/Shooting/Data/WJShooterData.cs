using UnityEngine;

namespace Assets.Scripts.WJ.Core.Shooting.Data
{
    [CreateAssetMenu(fileName = "WJShooterData", menuName = "WJ/Shooting/ShooterData")]
    public class WJShooterData : ScriptableObject
    {
        [Header("Basic Settings")]
        public GameObject bulletPrefab;
        public float fireRate = 0.5f;
        public float bulletSpeed = 20f;
        public float bulletDamage = 10f;
        public float bulletLifetime = 2f;

        [Header("Advanced Settings")]
        public float spread = 0f;
        public int bulletsPerShot = 1;
        public bool autoFire = false;
    }
}
