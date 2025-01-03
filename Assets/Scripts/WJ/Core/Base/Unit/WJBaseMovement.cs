using UnityEngine;

namespace WJ.Core.Base.Unit
{
    [RequireComponent(typeof(Rigidbody))]
    public class WJBaseMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] protected float moveSpeed = 5f;
        [SerializeField] protected float jumpForce = 5f;

        protected Rigidbody rb;
        protected Vector2 currentInput;
        protected bool isGrounded;

        [Header("Ground Check")]
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected float groundCheckDistance = 0.1f;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        public virtual void Move(Vector2 input)
        {
            currentInput = input;
        }

        public virtual void Jump()
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        protected virtual void FixedUpdate()
        {
            if (rb == null) return;
            HandleMovement();
            CheckGrounded();
        }

        protected virtual void CheckGrounded()
        {
            isGrounded = Physics.Raycast(
                transform.position + Vector3.up * 0.1f,
                Vector3.down,
                groundCheckDistance + 0.1f,
                groundLayer
            );
        }

        protected virtual void HandleMovement()
        {
            Vector3 movement = new Vector3(currentInput.x, 0, currentInput.y) * moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            
            if (movement != Vector3.zero)
            {
                transform.forward = movement.normalized;
            }
        }
    }
}
