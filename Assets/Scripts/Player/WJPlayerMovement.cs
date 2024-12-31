using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class WJPlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    // Constants for drop animation
    private const float DROP_HEIGHT = 5f;
    private const float DROP_DURATION = 2f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float bounceForce = 10f;
    
    private WJPlayerControls playerControls;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float networkLag;
    private WJMapManager mapManager;
    private bool isDropping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerControls = new WJPlayerControls();
        
        // Subscribe to the Move action
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;

        // Find MapManager in scene
        mapManager = FindObjectOfType<WJMapManager>();
        if (mapManager == null)
        {
            Debug.LogWarning("MapManager not found in scene!");
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Start drop sequence
            StartCoroutine(StartDropSequence());
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        playerControls.Enable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        playerControls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!isDropping)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    private IEnumerator StartDropSequence()
    {
        isDropping = true;
        playerControls.Disable();
        
        // Store initial position and calculate drop start position
        Vector3 endPos = transform.position;
        Vector3 startPos = endPos + Vector3.up * DROP_HEIGHT;
        
        // Disable rigidbody physics during drop
        rb.isKinematic = true;
        transform.position = startPos;

        float elapsed = 0f;
        
        while (elapsed < DROP_DURATION)
        {
            // Calculate smooth drop progress
            float progress = elapsed / DROP_DURATION;
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            
            // Update position
            transform.position = Vector3.Lerp(startPos, endPos, smoothProgress);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is exact
        transform.position = endPos;
        
        // Re-enable physics and input
        rb.isKinematic = false;
        playerControls.Enable();
        isDropping = false;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine || isDropping) return;

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

        // Rotate to face movement direction
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine) return;

        GameObject hitObject = collision.gameObject;

        // Handle rebound walls
        if (hitObject.CompareTag("ReboundWall"))
        {
            if (mapManager != null && mapManager.IsWallBounceEnabled(hitObject.name))
            {
                Vector3 bounceDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
                rb.velocity = bounceDirection * bounceForce;
            }
        }
        // Handle missile walls
        else if (hitObject.CompareTag("MissileWall"))
        {
            // Future implementation: Handle missile wall specific behavior
            Debug.Log("Hit missile wall: " + hitObject.name);
        }
        // Handle floor
        else if (hitObject.CompareTag("Floor"))
        {
            // Future implementation: Handle floor specific behavior
            Debug.Log("Hit floor: " + hitObject.name);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(moveInput);
            stream.SendNext(isDropping);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            moveInput = (Vector2)stream.ReceiveNext();
            isDropping = (bool)stream.ReceiveNext();
            networkLag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // Smoothly interpolate position for remote players
            rb.position = Vector3.Lerp(rb.position, networkPosition, Time.deltaTime * 10);
            rb.rotation = Quaternion.Lerp(rb.rotation, networkRotation, Time.deltaTime * 10);
        }
    }
}
