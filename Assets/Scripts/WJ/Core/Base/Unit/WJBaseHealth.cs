using UnityEngine;

namespace WJ.Core.Base.Unit
{
    public class WJBaseHealth : MonoBehaviour
    {
        [SerializeField] protected float maxHealth = 100f;
        protected float currentHealth;
        protected bool isDead;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;

            currentHealth = Mathf.Max(0, currentHealth - damage);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            isDead = true;
        }

        public virtual bool IsDead()
        {
            return isDead;
        }
    }
} 