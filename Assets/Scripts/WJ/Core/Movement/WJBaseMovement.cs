using UnityEngine;
using WJ.Core.Movement.Data;

namespace Assets.Scripts.WJ.Core.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class WJBaseMovement : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] protected WJMovementData movementData;

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
            else
            {
                Debug.LogError("Rigidbody component not found!");
            }
        }

        public virtual void Move(Vector2 input)
        {
            currentInput = input;
            Debug.Log($"WJBaseMovement.Move received input: {input}");
        }

        protected virtual void FixedUpdate()
        {
            if (movementData == null)
            {
                Debug.LogError("Movement Data is missing!");
                return;
            }
            
            if (rb == null)
            {
                Debug.LogError("Rigidbody is missing!");
                return;
            }

            float currentYVelocity = rb.velocity.y;
            Vector3 movement = new Vector3(currentInput.x, 0, currentInput.y) * movementData.moveSpeed;
            
            rb.velocity = new Vector3(movement.x, currentYVelocity, movement.z);
            Debug.Log($"Applied velocity: {rb.velocity}, Input: {currentInput}");
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
            float currentYVelocity = rb.velocity.y;
            Vector3 movement = new Vector3(currentInput.x, 0, currentInput.y) * 
                             movementData.moveSpeed;
            
            rb.velocity = new Vector3(movement.x, currentYVelocity, movement.z);
            
            if (movement != Vector3.zero)
            {
                transform.forward = Vector3.Lerp(
                    transform.forward,
                    movement.normalized,
                    movementData.rotationSpeed * Time.fixedDeltaTime
                );
            }
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Vector3 rayStart = transform.position + Vector3.up * 0.1f;
            Gizmos.DrawLine(rayStart, rayStart + Vector3.down * (groundCheckDistance + 0.1f));
        }
    }
}
