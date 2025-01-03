using UnityEngine;

namespace WJ.Core.Base.Unit
{
    public class WJBaseShooter : MonoBehaviour
    {
        [Header("Shooter Settings")]
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected float fireRate = 0.5f;
        [SerializeField] protected float spread = 0f;

        protected float nextFireTime;

        public virtual bool CanShoot()
        {
            return Time.time >= nextFireTime;
        }

        public virtual void Shoot(Vector3 direction)
        {
            if (!CanShoot() || bulletPrefab == null || firePoint == null) return;

            Vector3 spreadDirection = ApplySpread(direction);
            CreateBullet(spreadDirection);
            nextFireTime = Time.time + fireRate;
        }

        protected virtual Vector3 ApplySpread(Vector3 direction)
        {
            if (spread <= 0) return direction;

            float randomSpread = Random.Range(-spread, spread);
            Quaternion rotation = Quaternion.Euler(0, randomSpread, 0);
            return rotation * direction;
        }

        protected virtual void CreateBullet(Vector3 direction)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
            WJBaseBullet baseBullet = bullet.GetComponent<WJBaseBullet>();
            
            if (baseBullet != null)
            {
                baseBullet.Initialize(direction);
            }
        }

        public virtual void SetFireRate(float rate)
        {
            fireRate = rate;
        }
    }
} 