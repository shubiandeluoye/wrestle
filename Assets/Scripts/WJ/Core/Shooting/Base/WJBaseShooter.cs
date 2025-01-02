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
        [SerializeField] protected bool isLeftPlayer = true;

        protected float nextFireTime;

        public virtual bool TryShoot(float angle)
        {
            if (Time.time < nextFireTime || shooterData == null) 
                return false;

            GameObject bullet = Instantiate(
                shooterData.bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            if (bullet.TryGetComponent<WJBaseBullet>(out var bulletComponent))
            {
                var playerController = GetComponentInParent<WJPlayerController>();
                int shooterId = playerController != null ? playerController.GetPlayerId() : -1;

                bulletComponent.Initialize(angle, shooterId);
                WJAudioManager.Instance?.PlayShootSound();
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
