using UnityEngine;

namespace Assets.Scripts.WJ.Core.Shooting.Data
{
    [CreateAssetMenu(fileName = "WJBulletData", menuName = "WJ/Combat/BulletData")]
    public class WJBulletData : ScriptableObject
    {
        public enum BulletType
        {
            Normal,     // 普通子弹
            Explosive,  // 爆炸子弹
            Penetrating // 穿透子弹
        }

        [Header("Basic Settings")]
        public BulletType type;
        public float damage = 10f;
        public float speed = 20f;
        public float lifeTime = 2f;

        [Header("Visual Effects")]
        public GameObject bulletModel;        // 子弹模型
        public ParticleSystem muzzleEffect;   // 枪口特效
        public ParticleSystem trailEffect;    // 拖尾特效
        public TrailRenderer bulletTrail;     // 子弹拖尾
        public ParticleSystem hitEffect;      // 击中特效
        
        [Header("Type Specific Effects")]
        public ParticleSystem explosionEffect;    // 爆炸特效
        public ParticleSystem penetrationEffect;  // 穿透特效

        [Header("Audio")]
        public AudioClip shootSound;          // 射击音效
        public AudioClip hitSound;            // 击中音效
        public float soundVolume = 1f;

        [Header("Type Specific Settings")]
        // 爆炸子弹设置
        public float explosionRadius = 3f;
        public float explosionForce = 500f;
        
        // 穿透子弹设置
        public int penetrationCount = 3;
        public float damageReductionPerHit = 0.2f;
    }
} 