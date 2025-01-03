using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.WJ.Core.Shooting.Base;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Game
{
    public class BulletCollector : MonoBehaviour
    {
        [SerializeField] private float explosionTimer = 10f;    // 爆炸计时器
        [SerializeField] private int maxBullets = 5;           // 最大收集子弹数
        [SerializeField] private float explosionRadius = 5f;    // 爆炸范围
        [SerializeField] private float explosionForce = 10f;    // 爆炸力度

        private List<WJBaseBullet> collectedBullets = new List<WJBaseBullet>();
        private float currentTimer;
        private bool isExploded;

        private void Start()
        {
            currentTimer = explosionTimer;
        }

        private void Update()
        {
            if (isExploded) return;

            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0 || collectedBullets.Count >= maxBullets)
            {
                Explode();
            }
        }

        public void CollectBullet(WJBaseBullet bullet)
        {
            if (!isExploded && !bullet.IsCollected())
            {
                collectedBullets.Add(bullet);
            }
        }

        private void Explode()
        {
            isExploded = true;

            // 统计每个队伍的子弹数量
            int team1Bullets = 0, team2Bullets = 0;
            foreach (var bullet in collectedBullets)
            {
                if (bullet.GetTeamId() == 1) team1Bullets++;
                else team2Bullets++;
            }

            // 决定爆炸影响哪一方
            bool affectLeftTeam = team1Bullets < team2Bullets;

            // 寻找范围内的玩家并造成伤害
            var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    var player = col.GetComponent<WJPlayerController>();
                    if (player != null && player.GetIsLeftPlayer() == affectLeftTeam)
                    {
                        // 造成伤害
                        var scoreManager = FindObjectOfType<WJScoreManager>();
                        scoreManager?.DeductScore(affectLeftTeam);

                        // 添加爆炸力
                        var rb = col.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                        }
                    }
                }
            }

            // 销毁收集器
            Destroy(gameObject);
        }
    }
} 