using UnityEngine;

namespace WJ.Core.Base.Unit
{
    public class WJBaseBullet : MonoBehaviour
    {
        [Header("Bullet Settings")]
        [SerializeField] protected float speed = 20f;
        [SerializeField] protected float lifeTime = 5f;
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected int bounceCount = 0;    // 反弹次数
        [SerializeField] protected LayerMask bounceLayer;  // 可反弹的层级

        protected Vector3 direction;
        protected bool isInitialized;
        protected int currentBounceCount;

        public virtual void Initialize(Vector3 direction)
        {
            this.direction = direction.normalized;
            isInitialized = true;
            currentBounceCount = bounceCount;
            Destroy(gameObject, lifeTime);
        }

        protected virtual void Update()
        {
            if (!isInitialized) return;
            transform.position += direction * speed * Time.deltaTime;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            // 检查是否可以反弹
            if (currentBounceCount > 0 && ((1 << other.gameObject.layer) & bounceLayer) != 0)
            {
                HandleBounce(other);
            }
            else
            {
                HandleCollision(other);
            }
        }

        protected virtual void HandleBounce(Collider other)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
            {
                direction = Vector3.Reflect(direction, hit.normal);
                currentBounceCount--;
            }
        }

        protected virtual void HandleCollision(Collider other)
        {
            Destroy(gameObject);
        }

        public virtual float GetDamage()
        {
            return damage;
        }
    }
}
