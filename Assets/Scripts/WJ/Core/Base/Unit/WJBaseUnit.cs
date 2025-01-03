using UnityEngine;

namespace WJ.Core.Base.Unit
{
    [RequireComponent(typeof(WJBaseMovement))]
    [RequireComponent(typeof(WJBaseHealth))]
    public class WJBaseUnit : MonoBehaviour
    {
        protected WJBaseMovement movement;
        protected WJBaseHealth health;
        protected WJBaseShooter shooter;

        protected virtual void Awake()
        {
            movement = GetComponent<WJBaseMovement>();
            health = GetComponent<WJBaseHealth>();
            shooter = GetComponent<WJBaseShooter>();
        }

        public virtual void Move(Vector2 input)
        {
            if (movement != null)
            {
                movement.Move(input);
            }
        }

        public virtual void Shoot(Vector3 direction)
        {
            if (shooter != null)
            {
                shooter.Shoot(direction);
            }
        }

        public virtual void TakeDamage(float damage)
        {
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
