using UnityEngine;
using Assets.Scripts.WJ.Core.Audio;
using Assets.Scripts.WJ.Core.Player.Controllers;
using UnityEngine.InputSystem;

namespace Assets.Scripts.WJ.Core.Shooting.Base
{
    public class WJBaseShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Transform firePoint;
        
        [Header("Settings")]
        [SerializeField] protected WJShooterData shooterData;
        [SerializeField] protected bool isLeftPlayer = true;

        [Header("Angle Settings")]
        [SerializeField] private bool useWideAngle = false;  // 是否使用45度（否则使用30度）
        private float rightAngle30 = 30f;   // 向右30度
        private float leftAngle30 = -30f;   // 向左30度
        private float rightAngle45 = 45f;   // 向右45度
        private float leftAngle45 = -45f;   // 向左45度
        private float straightAngle = 0f;    // 直射

        [SerializeField] protected float nextFireTime;

        [SerializeField] protected WJAudioManager audioManager;  // 添加音频管理器引用

        // 按键响应
        public void OnShootRight()  // N键 - 向右30/45度射击
        {
            float shootAngle = useWideAngle ? rightAngle45 : rightAngle30;
            TryShoot(shootAngle);
        }

        public void OnShootLeft()   // K键 - 向左30/45度射击
        {
            float shootAngle = useWideAngle ? leftAngle45 : leftAngle30;
            TryShoot(shootAngle);
        }

        public void OnShootStraight()  // J键 - 直线射击
        {
            TryShoot(straightAngle);
        }

        public void OnToggleAngleRange()  // M键 - 切换30/45度
        {
            useWideAngle = !useWideAngle;
            Debug.Log($"Toggled to {(useWideAngle ? "45" : "30")} degree range");
        }

        public virtual bool TryShoot(float angle)
        {
            if (Time.time < nextFireTime || shooterData == null) 
                return false;

            // 基准方向为90度（向上），加上输入的角度
            float finalAngle = 90f + angle;
            
            GameObject bullet = Instantiate(
                shooterData.bulletPrefab,
                firePoint.position,
                Quaternion.Euler(0, finalAngle, 0)
            );

            if (bullet.TryGetComponent<WJBaseBullet>(out var bulletComponent))
            {
                int shooterId = GetComponentInParent<WJPlayerController>().GetPlayerId();
                bulletComponent.Initialize(finalAngle, shooterId);
                
                if (audioManager != null)
                {
                    audioManager.PlayShootSound();
                }
            }

            nextFireTime = Time.time + shooterData.fireRate;
            return true;
        }

        protected virtual void OnDrawGizmos()
        {
            if (firePoint != null)
            {
                // 显示所有可能的射击角度
                Gizmos.color = Color.yellow;  // 直射线
                DrawAngleLine(90f + straightAngle);
                
                Gizmos.color = Color.gray;   // 左右角度线
                if (useWideAngle)
                {
                    DrawAngleLine(90f + rightAngle45);
                    DrawAngleLine(90f + leftAngle45);
                }
                else
                {
                    DrawAngleLine(90f + rightAngle30);
                    DrawAngleLine(90f + leftAngle30);
                }
            }
        }

        private void DrawAngleLine(float angle)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Gizmos.DrawRay(firePoint.position, direction * 2f);
        }
    }
}
