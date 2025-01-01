using UnityEngine;
using WJ.Core.Movement.Data;

namespace WJ.Core.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class WJBaseMovement : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] protected WJMovementData movementData;

        protected Rigidbody rb;
        protected CapsuleCollider capsuleCollider;
        protected Vector2 currentInput;
        protected bool isGrounded;
        protected bool isMoving;

        protected virtual void Awake()
        {
            InitializeComponents();
        }

        protected virtual void InitializeComponents()
        {
            rb = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            if (rb != null)
            {
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

            // 验证数据配置
            if (movementData == null)
            {
                Debug.LogError("Movement Data is not assigned!");
            }
        }

        public virtual void Move(Vector2 input)
        {
            currentInput = input;
            isMoving = input.magnitude > 0.1f;
        }

        protected virtual void FixedUpdate()
        {
            if (movementData == null) return;
            
            CheckGround();
            HandleMovement();
        }

        protected virtual void HandleMovement()
        {
            if (rb == null) return;

            if (isMoving)
            {
                Vector3 movement = new Vector3(currentInput.x, 0f, currentInput.y);
                
                // 使用加速度计算目标速度
                Vector3 targetVelocity = movement.normalized * movementData.moveSpeed;
                Vector3 currentVelocity = rb.velocity;
                currentVelocity.y = 0f; // 保持垂直速度不变

                // 在空中时使用空中控制系数
                float speedFactor = isGrounded ? 1f : movementData.airControlFactor;
                targetVelocity *= speedFactor;

                // 使用加速度插值到目标速度
                Vector3 newVelocity = Vector3.MoveTowards(
                    currentVelocity,
                    targetVelocity,
                    movementData.acceleration * Time.fixedDeltaTime
                );

                // 限制最大速度
                if (newVelocity.magnitude > movementData.maxSpeed)
                {
                    newVelocity = newVelocity.normalized * movementData.maxSpeed;
                }

                // 应用最终速度
                newVelocity.y = rb.velocity.y;
                rb.velocity = newVelocity;

                // 处理转向
                if (movement != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(movement);
                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        targetRotation,
                        movementData.rotationSpeed * Time.fixedDeltaTime
                    );
                }
            }
            else
            {
                // 使用减速度停止
                Vector3 currentVelocity = rb.velocity;
                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0f, movementData.deceleration * Time.fixedDeltaTime);
                currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, 0f, movementData.deceleration * Time.fixedDeltaTime);
                rb.velocity = currentVelocity;
            }
        }

        protected virtual void CheckGround()
        {
            if (capsuleCollider == null) return;

            RaycastHit hit;
            Vector3 rayStart = transform.position - new Vector3(0f, capsuleCollider.height/2, 0f);
            bool wasGrounded = isGrounded;
            
            if (Physics.Raycast(rayStart, Vector3.down, out hit, movementData.groundCheckDistance))
            {
                isGrounded = hit.collider.CompareTag(movementData.groundTag);
                if (!wasGrounded && isGrounded)
                {
                    OnLanded();
                }
            }
            else
            {
                isGrounded = false;
            }
        }

        protected virtual void OnLanded()
        {
            Vector3 currentVelocity = rb.velocity;
            rb.velocity = new Vector3(
                currentVelocity.x,
                0f,
                currentVelocity.z
            );
        }

        protected virtual void OnDrawGizmos()
        {
            if (!Application.isPlaying || movementData == null) return;
            
            CapsuleCollider capsule = GetComponent<CapsuleCollider>();
            if (capsule == null) return;

            Gizmos.color = isGrounded ? Color.green : Color.red;
            Vector3 rayStart = transform.position - new Vector3(0f, capsule.height/2, 0f);
            Gizmos.DrawLine(rayStart, rayStart + Vector3.down * movementData.groundCheckDistance);
        }
    }
}
