using UnityEngine;
using Assets.Scripts.WJ.Core.Game;
using Assets.Scripts.WJ.Core.Player.Controllers;
using Assets.Scripts.WJ.Core.Audio;

namespace Assets.Scripts.WJ.Core.Shooting.Base
{
    [RequireComponent(typeof(Rigidbody))]
    public class WJBaseBullet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float maxDistance = 30f;
        [SerializeField] private int maxBounces = 3;

        private Vector3 direction;
        private Vector3 startPosition;
        private Rigidbody rb;
        private float startTime;
        private int bounceCount = 0;

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

        public void Initialize(float angle, bool isLeftPlayer)
        {
            startPosition = transform.position;
            startTime = Time.time;
            
            // 基础方向
            float directionMultiplier = isLeftPlayer ? 1f : -1f;
            
            // 计算发射方向
            switch (angle)
            {
                case 0:  // 水平
                    direction = new Vector3(directionMultiplier, 0, 0);
                    break;
                case 30:  // 上30度
                    direction = new Vector3(directionMultiplier * 0.866f, 0, 0.5f);
                    break;
                case -30:  // 下30度
                    direction = new Vector3(directionMultiplier * 0.866f, 0, -0.5f);
                    break;
                case 45:  // 上45度
                    direction = new Vector3(directionMultiplier * 0.707f, 0, 0.707f);
                    break;
                case -45:  // 下45度
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
            if (collision.gameObject.CompareTag("Player"))
            {
                WJAudioManager.Instance?.PlayHitSound();  // 播放击中声音
                var scoreManager = FindObjectOfType<WJNetworkScoreManager>();
                if (scoreManager != null)
                {
                    var playerController = collision.gameObject.GetComponent<WJPlayerController>();
                    if (playerController != null)
                    {
                        // 被击中的玩家扣分
                        scoreManager.DeductScore(playerController.IsLeftPlayer);
                    }
                }
                Destroy(gameObject);
                return;
            }

            // 达到最大反弹次数，销毁子弹
            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
                return;
            }

            // 处理反弹
            WJAudioManager.Instance?.PlayBounceSound();  // 播放反弹声音
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
            // 超出最大距离销毁
            float distance = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(startPosition.x, 0, startPosition.z)
            );
            
            if (distance > maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
