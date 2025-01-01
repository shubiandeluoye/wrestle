using UnityEngine;
using WJ.Core.Shooting.Data;

namespace WJ.Core.Shooting.Base
{
    public class WJBaseShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected GameObject bulletPrefab;
        
        [Header("Settings")]
        [SerializeField] protected WJShooterData shooterData;
        [SerializeField] protected bool canShoot = true;
        [SerializeField] protected bool isLeftPlayer = true;

        protected float nextFireTime;

        public virtual void TryShoot(float angle)
        {
            if (!canShoot || Time.time < nextFireTime || shooterData == null) return;

            if (firePoint != null && bulletPrefab != null)
            {
                // 计算散布
                Quaternion spread = CalculateSpread();
                
                // 实例化子弹
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, 
                    Quaternion.Euler(0, 0, angle) * spread);
                
                // 设置子弹速度
                if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
                {
                    Vector2 direction = Quaternion.Euler(0, 0, angle) * 
                        (isLeftPlayer ? Vector2.right : Vector2.left);
                    
                    bulletRb.velocity = new Vector3(direction.x, direction.y, 0) * 
                        shooterData.bulletSpeed;
                }

                // 更新下次射击时间
                nextFireTime = Time.time + shooterData.fireRate;
            }
        }

        protected virtual Quaternion CalculateSpread()
        {
            if (shooterData == null || shooterData.spread <= 0) 
                return Quaternion.identity;

            float spreadX = Random.Range(-shooterData.spread, shooterData.spread);
            float spreadY = Random.Range(-shooterData.spread, shooterData.spread);
            
            return Quaternion.Euler(spreadX, spreadY, 0f);
        }

        protected virtual void OnDrawGizmos()
        {
            if (firePoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(firePoint.position, firePoint.forward * 2f);
            }
        }
    }
}
