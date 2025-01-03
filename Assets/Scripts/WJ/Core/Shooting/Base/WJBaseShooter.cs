using UnityEngine;
using Assets.Scripts.WJ.Core.Audio;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Shooting.Base
{
    public class WJBaseShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Transform firePoint;
        
        [Header("Settings")]
        [SerializeField] protected WJShooterData shooterData;
        [SerializeField] protected bool canShoot = true;
        
        [Header("Bullet Settings")]
        [SerializeField] [Range(0.2f, 2f)] protected float bulletScaleRatio = 0.5f;  // 增大范围到0.2-2倍
        [Tooltip("子弹大小相对于玩家的比例(0.2-2倍)")]

        protected float nextFireTime;

        public virtual bool TryShoot(float angle)
        {
            if (Time.time < nextFireTime || shooterData == null) 
                return false;

            var playerController = GetComponentInParent<WJPlayerController>();
            if (playerController == null) return false;

            // 获取玩家大小并计算子弹大小
            Vector3 playerScale = transform.root.localScale;
            Vector3 bulletScale = playerScale * bulletScaleRatio;

            GameObject bullet = Instantiate(
                shooterData.bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            bullet.transform.localScale = bulletScale;

            if (bullet.TryGetComponent<WJBaseBullet>(out var bulletComponent))
            {
                int shooterId = playerController.GetPlayerId();
                bulletComponent.Initialize(angle, shooterId);
                
                // 添加空值检查
                if (WJAudioManager.Instance != null)
                {
                    WJAudioManager.Instance.PlayShootSound();
                }
            }

            nextFireTime = Time.time + shooterData.fireRate;
            return true;
        }

        protected virtual void OnDrawGizmos()
        {
            if (firePoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(firePoint.position, Vector3.right);
                Gizmos.color = Color.green;
                Gizmos.DrawRay(firePoint.position, Vector3.up);
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(firePoint.position, Vector3.forward);
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(firePoint.position, firePoint.forward * 2f);
            }
        }
    }
}
