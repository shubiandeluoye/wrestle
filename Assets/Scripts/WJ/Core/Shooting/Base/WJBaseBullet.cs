using UnityEngine;
using Assets.Scripts.WJ.Core.Game;
using Assets.Scripts.WJ.Core.Audio;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Shooting.Base
{
    [RequireComponent(typeof(Rigidbody))]
    public class WJBaseBullet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float maxDistance = 50f;
        [SerializeField] private int maxBounces = 3;

        private Vector3 direction;
        private Vector3 startPosition;
        private Rigidbody rb;
        private float startTime;
        private int bounceCount = 0;

        // 添加子弹归属相关字段
        private int teamId;           // 队伍ID：1=左队，2=右队
        private int shooterId;        // 发射者ID
        private bool canHitOwner;     // 是否可以击中发射者
        private bool isCollected;     // 是否被收集器收集

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezePositionY | 
                               RigidbodyConstraints.FreezeRotation;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.detectCollisions = true;
            }
        }

        public void Initialize(float angle, int shooterId, bool canHitOwner = false)
        {
            this.shooterId = shooterId;
            this.teamId = (shooterId % 2 == 1) ? 1 : 2;  // 根据ID判断队伍
            this.canHitOwner = canHitOwner;
            
            startPosition = transform.position;
            startTime = Time.time;
            
            // 根据ID判断方向：奇数ID向右，偶数ID向左
            float directionMultiplier = (shooterId % 2 == 1) ? 1f : -1f;
            
            switch (angle)
            {
                case 0:
                    direction = new Vector3(directionMultiplier, 0, 0);
                    break;
                case 30:
                    direction = new Vector3(directionMultiplier * 0.866f, 0, 0.5f);
                    break;
                case -30:
                    direction = new Vector3(directionMultiplier * 0.866f, 0, -0.5f);
                    break;
                case 45:
                    direction = new Vector3(directionMultiplier * 0.707f, 0, 0.707f);
                    break;
                case -45:
                    direction = new Vector3(directionMultiplier * 0.707f, 0, -0.707f);
                    break;
                default:
                    direction = new Vector3(directionMultiplier, 0, 0);
                    break;
            }

            direction = direction.normalized;
            transform.forward = direction;
            
            if (rb != null)
            {
                rb.velocity = direction * speed;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 检查是否碰到收集器
            if (collision.gameObject.CompareTag("Bullet"))
            {
                var collector = collision.gameObject.GetComponent<BulletCollector>();
                if (collector != null)
                {
                    collector.CollectBullet(this);
                    isCollected = true;
                    gameObject.SetActive(false);
                    return;
                }
            }

            // 检查是否碰到玩家
            if (collision.gameObject.CompareTag("Player"))
            {
                var playerController = collision.gameObject.GetComponent<WJPlayerController>();
                if (playerController != null)
                {
                    // 获取被击中玩家的ID和队伍
                    int hitPlayerId = playerController.GetPlayerId();
                    bool isLeftTeam = playerController.GetIsLeftPlayer();
                    int hitTeamId = isLeftTeam ? 1 : 2;

                    // 判断是否可以造成伤害
                    bool canDamage = true;
                    if (hitPlayerId == shooterId && !canHitOwner)
                    {
                        canDamage = false;  // 不能伤害发射者
                    }
                    if (hitTeamId == teamId && !canHitOwner)
                    {
                        canDamage = false;  // 不能伤害队友
                    }

                    if (canDamage)
                    {
                        if (WJAudioManager.Instance != null)
                        {
                            WJAudioManager.Instance.PlayHitSound();
                        }
                        var scoreManager = FindObjectOfType<WJScoreManager>();
                        scoreManager?.DeductScore(isLeftTeam);
                    }
                    
                    Destroy(gameObject);
                    return;
                }
            }

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
                return;
            }

            if (WJAudioManager.Instance != null)
            {
                WJAudioManager.Instance.PlayBounceSound();
            }
            Vector3 normal = collision.contacts[0].normal;
            normal.y = 0;
            normal.Normalize();
            
            Vector3 reflectDir = Vector3.Reflect(direction, normal);
            reflectDir.y = 0;
            reflectDir.Normalize();
            
            transform.position += normal * 0.1f;
            direction = reflectDir;
            transform.forward = direction;
            
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.velocity = direction * speed;
                rb.angularVelocity = Vector3.zero;
            }

            bounceCount++;
        }

        private void Update()
        {
            float distance = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(startPosition.x, 0, startPosition.z)
            );
            
            if (distance > maxDistance)
            {
                Destroy(gameObject);
            }
        }

        // 获取子弹信息的方法
        public int GetTeamId() => teamId;
        public int GetShooterId() => shooterId;
        public bool IsCollected() => isCollected;
    }
}
