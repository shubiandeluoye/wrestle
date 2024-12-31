using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class WJPlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    // Constants for drop animation and movement
    private const float DROP_HEIGHT = 5f;
    private const float DROP_DURATION = 2f;
    private const float MOVE_SPEED = 5f;

    [Header("Movement Settings")]
    public float bounceForce = 10f;
    
    private WJPlayerControls playerControls;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float networkLag;
    private WJMapManager mapManager;
    private bool isDropping = false;
    private bool isGrounded = false;

    private bool IsGrounded()
    {
        // Raycast down slightly from player's position
        RaycastHit hit;
        bool wasGrounded = isGrounded;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            bool newGrounded = hit.collider.CompareTag("Floor");
            // Log state transitions for debugging
            if (wasGrounded != newGrounded)
            {
                Debug.Log($"Ground state changed: {(newGrounded ? "Landed" : "Left ground")} at position {transform.position}");
            }
            return newGrounded;
        }
        // Log when leaving ground without floor below
        if (wasGrounded)
        {
            Debug.Log($"Left ground (no floor detected) at position {transform.position}");
        }
        return false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        // Configure Rigidbody for smooth movement
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
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

    private bool hasStartedGame = false;
    
    private void Start()
    {
        if (photonView.IsMine && !hasStartedGame)
        {
            hasStartedGame = true;
            // Start drop sequence only once at game start
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

    private Vector3 CalculateMovement(Vector2 input)
    {
        // Convert input to 3D movement vector
        return new Vector3(input.x, 0f, input.y);
    }

    private void ApplyMovement(Vector3 movement)
    {
        // Use velocity-based movement for both ground and air states
        // This maintains smooth physics simulation and prevents teleportation
        Vector3 desiredVelocity = new Vector3(movement.x * MOVE_SPEED, rb.velocity.y, movement.z * MOVE_SPEED);
        rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity, 0.2f);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine || isDropping) return;

        // Update grounded state
        isGrounded = IsGrounded();

        // Calculate and apply movement
        Vector3 movement = CalculateMovement(moveInput);
        ApplyMovement(movement);

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
