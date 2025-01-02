using UnityEngine;
using Assets.Scripts.WJ.Core.Audio;

namespace Assets.Scripts.WJ.Core.Shooting.Base
{
    public class WJBaseShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Transform firePoint;
        
        [Header("Settings")]
        [SerializeField] protected WJShooterData shooterData;
        [SerializeField] protected bool canShoot = true;
        [SerializeField] protected bool isLeftPlayer = true;

        protected float nextFireTime;

        public virtual bool TryShoot(float angle)
        {
            if (Time.time < nextFireTime || shooterData == null) 
            {
                Debug.Log($"[Shooter] Cannot shoot: Time={Time.time}, NextFire={nextFireTime}, HasData={shooterData != null}");
                return false;
            }

            Debug.Log($"[Shooter] Attempting to shoot at angle: {angle}");
            
            GameObject bullet = Instantiate(
                shooterData.bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            if (bullet.TryGetComponent<WJBaseBullet>(out var bulletComponent))
            {
                bulletComponent.Initialize(angle, isLeftPlayer);
                WJAudioManager.Instance?.PlayShootSound();
                Debug.Log($"[Shooter] Bullet initialized with angle: {angle}, isLeftPlayer: {isLeftPlayer}");
            }
            else
            {
                Debug.LogError("[Shooter] WJBaseBullet component not found on bullet prefab!");
            }

            nextFireTime = Time.time + shooterData.fireRate;
            return true;
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
