using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class WJPlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    // 常量定义
    private const float DROP_HEIGHT = 5f;
    private const float DROP_DURATION = 1f;

    [Header("Movement Settings")]
    [SerializeField] [Range(1f, 10f)] private float moveSpeed = 5f;
    [SerializeField] [Range(0f, 20f)] private float rotationSpeed = 15f;

    [Header("Physics Settings")]
    [SerializeField] [Range(0f, 5f)] private float dragForce = 2f;
    [SerializeField] [Range(0.1f, 5f)] private float mass = 1f;
    [SerializeField] [Range(1f, 20f)] private float bounceForce = 10f;

    [Header("Network Smoothing")]
    [SerializeField] [Range(1f, 20f)] private float smoothSpeed = 10f;
    [SerializeField] [Range(1f, 20f)] private float rotationSmoothSpeed = 10f;
    [SerializeField] [Range(0.01f, 1f)] private float positionThreshold = 0.1f;

    private Rigidbody rb;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float networkLag;
    private WJMapManager mapManager;
    private bool isDropping = false;
    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.drag = dragForce;
        rb.mass = mass;

        mapManager = FindObjectOfType<WJMapManager>();
    }

    private void Update()
    {
        // 处理网络同步
        if (!photonView.IsMine)
        {
            // 只有当位置差异超过阈值时才进行同步
            float distance = Vector3.Distance(rb.position, networkPosition);
            if (distance > positionThreshold)
            {
                // 使用平滑插值进行位置同步
                rb.MovePosition(Vector3.Lerp(rb.position, networkPosition, smoothSpeed * Time.deltaTime));
                // 使用平滑插值进行旋转同步
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, networkRotation, rotationSmoothSpeed * Time.deltaTime));
            }
            return;
        }

        if (isDropping) return;

        // 检查是否在地面上
        if (!isGrounded)
        {
            // 如果不在地面上，可以添加额外的处理
            // 比如限制移动或者应用重力
            return;
        }

        // 使用传统输入系统
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 确保只能一个方向移动
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            vertical = 0;
        }

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        
        if (movement != Vector3.zero)
        {
            movement.Normalize();
            Vector3 targetPosition = rb.position + movement * moveSpeed * Time.deltaTime;
            rb.MovePosition(targetPosition);

            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine) return;

        GameObject hitObject = collision.gameObject;

        if (hitObject.CompareTag("ReboundWall"))
        {
            if (mapManager != null && mapManager.IsWallBounceEnabled(hitObject.name))
            {
                Vector3 bounceDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
                rb.velocity = bounceDirection * bounceForce;
            }
        }
        else if (hitObject.CompareTag("Floor"))
        {
            isGrounded = true;  // 设置接地状态
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;  // 离开地面时更新状态
        }
    }

    // 网络同步相关代码
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(isDropping);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            rb.velocity = (Vector3)stream.ReceiveNext();
            isDropping = (bool)stream.ReceiveNext();
            networkLag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        }
    }

    private IEnumerator StartDropSequence()
    {
        isDropping = true;
        float startTime = Time.time;
        Vector3 startPos = new Vector3(transform.position.x, DROP_HEIGHT, transform.position.z);
        Vector3 endPos = new Vector3(transform.position.x, 0f, transform.position.z);

        while (Time.time - startTime < DROP_DURATION)
        {
            float t = (Time.time - startTime) / DROP_DURATION;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        isDropping = false;
    }
}
