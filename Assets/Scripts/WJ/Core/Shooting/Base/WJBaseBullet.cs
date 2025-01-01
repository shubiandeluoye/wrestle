using UnityEngine;

namespace WJ.Core.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class WJBaseBullet : MonoBehaviour
    {
        [SerializeField] protected WJBulletData bulletData;
        
        protected Rigidbody rb;
        protected float currentDamage;
        protected int hitsRemaining;
        protected float spawnTime;

        protected ParticleSystem activeTrailEffect;
        protected TrailRenderer activeTrailRenderer;
        protected AudioSource audioSource;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            InitializeBullet();
        }

        protected virtual void InitializeBullet()
        {
            if (bulletData == null) return;

            // 基础设置
            currentDamage = bulletData.damage;
            spawnTime = Time.time;
            hitsRemaining = bulletData.type == WJBulletData.BulletType.Penetrating ? 
                           bulletData.penetrationCount : 1;

            // 设置子弹模型
            if (bulletData.bulletModel != null)
            {
                Instantiate(bulletData.bulletModel, transform);
            }

            // 添加拖尾特效
            if (bulletData.trailEffect != null)
            {
                activeTrailEffect = Instantiate(bulletData.trailEffect, transform);
            }

            // 添加拖尾渲染器
            if (bulletData.bulletTrail != null)
            {
                activeTrailRenderer = Instantiate(bulletData.bulletTrail, transform);
            }

            // 设置音频
            SetupAudio();
        }

        protected virtual void SetupAudio()
        {
            if (bulletData.hitSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.volume = bulletData.soundVolume;
            }
        }

        protected virtual void Update()
        {
            if (Time.time - spawnTime >= bulletData.lifeTime)
            {
                DestroyBullet();
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            switch (bulletData.type)
            {
                case WJBulletData.BulletType.Normal:
                    HandleNormalHit(collision);
                    break;

                case WJBulletData.BulletType.Explosive:
                    HandleExplosiveHit(collision);
                    break;

                case WJBulletData.BulletType.Penetrating:
                    HandlePenetratingHit(collision);
                    break;
            }
        }

        protected virtual void HandleNormalHit(Collision collision)
        {
            ApplyDamage(collision.gameObject);
            CreateHitEffect(collision);
            DestroyBullet();
        }

        protected virtual void HandleExplosiveHit(Collision collision)
        {
            // 创建爆炸效果
            CreateHitEffect(collision);

            // 检测范围内的物体
            Collider[] colliders = Physics.OverlapSphere(transform.position, bulletData.explosionRadius);
            foreach (Collider hit in colliders)
            {
                // 对范围内物体造成伤害
                ApplyDamage(hit.gameObject);

                // 添加爆炸力
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(bulletData.explosionForce, transform.position, bulletData.explosionRadius);
                }
            }

            DestroyBullet();
        }

        protected virtual void HandlePenetratingHit(Collision collision)
        {
            ApplyDamage(collision.gameObject);
            CreateHitEffect(collision);

            hitsRemaining--;
            currentDamage *= (1f - bulletData.damageReductionPerHit);

            if (hitsRemaining <= 0)
            {
                DestroyBullet();
            }
        }

        protected virtual void ApplyDamage(GameObject target)
        {
            // 这里添加伤害处理逻辑
            // 例如：target.GetComponent<Health>()?.TakeDamage(currentDamage);
        }

        protected virtual void CreateHitEffect(Collision collision)
        {
            Vector3 hitPoint = collision.contacts[0].point;
            Quaternion hitRotation = Quaternion.LookRotation(collision.contacts[0].normal);

            // 播放击中特效
            if (bulletData.hitEffect != null)
            {
                Instantiate(bulletData.hitEffect, hitPoint, hitRotation);
            }

            // 根据子弹类型播放特殊特效
            switch (bulletData.type)
            {
                case WJBulletData.BulletType.Explosive:
                    if (bulletData.explosionEffect != null)
                    {
                        Instantiate(bulletData.explosionEffect, hitPoint, hitRotation);
                    }
                    break;

                case WJBulletData.BulletType.Penetrating:
                    if (bulletData.penetrationEffect != null)
                    {
                        Instantiate(bulletData.penetrationEffect, hitPoint, hitRotation);
                    }
                    break;
            }

            // 播放击中音效
            if (audioSource != null && bulletData.hitSound != null)
            {
                audioSource.PlayOneShot(bulletData.hitSound);
            }
        }

        protected virtual void DestroyBullet()
        {
            Destroy(gameObject);
        }

        // 公共方法用于设置子弹数据
        public virtual void SetBulletData(WJBulletData data)
        {
            bulletData = data;
            InitializeBullet();
        }

        protected virtual void OnDestroy()
        {
            // 确保特效正确清理
            if (activeTrailEffect != null)
            {
                activeTrailEffect.transform.SetParent(null);
                var main = activeTrailEffect.main;
                main.stopAction = ParticleSystemStopAction.Destroy;
                activeTrailEffect.Stop();
            }

            if (activeTrailRenderer != null)
            {
                activeTrailRenderer.transform.SetParent(null);
                Destroy(activeTrailRenderer.gameObject, activeTrailRenderer.time);
            }
        }
    }
}
