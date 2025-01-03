using UnityEngine;

namespace WJ.Core.Base.Manager
{
    public abstract class WJBaseCameraManager : MonoBehaviour
    {
        protected static WJBaseCameraManager instance;
        public static WJBaseCameraManager Instance => instance;

        [Header("Camera Settings")]
        [SerializeField] protected Camera mainCamera;
        [SerializeField] protected bool smoothFollow = true;
        [SerializeField] protected float smoothSpeed = 5f;
        [SerializeField] protected Vector3 offset = new Vector3(0f, 10f, -10f);
        
        [Header("Camera Bounds")]
        [SerializeField] protected bool useBounds = false;
        [SerializeField] protected Vector2 minBounds;
        [SerializeField] protected Vector2 maxBounds;

        protected Transform target;
        protected Vector3 desiredPosition;
        protected Vector3 smoothedPosition;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeCamera();
            }
            else
            {
                Debug.LogWarning($"Multiple instances of {GetType().Name} detected. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        protected virtual void InitializeCamera()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    Debug.LogError($"{GetType().Name}: No main camera found in scene!");
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if (target != null && mainCamera != null)
            {
                UpdateCameraPosition();
            }
        }

        protected virtual void UpdateCameraPosition()
        {
            // Calculate desired position
            desiredPosition = target.position + offset;

            if (useBounds)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
            }

            if (smoothFollow)
            {
                // Smoothly move camera
                smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                mainCamera.transform.position = smoothedPosition;
            }
            else
            {
                mainCamera.transform.position = desiredPosition;
            }

            // Make camera look at target
            mainCamera.transform.LookAt(target);
        }

        public virtual void SetTarget(Transform newTarget)
        {
            if (newTarget == null)
            {
                Debug.LogWarning($"{GetType().Name}: Attempting to set null target!");
                return;
            }

            target = newTarget;
            Debug.Log($"{GetType().Name}: Camera target set to {target.name}");
        }

        public virtual void SetPosition(Vector3 position)
        {
            if (mainCamera != null)
            {
                mainCamera.transform.position = position;
                Debug.Log($"{GetType().Name}: Camera position set to {position}");
            }
            else
            {
                Debug.LogError($"{GetType().Name}: Cannot set position - no camera assigned!");
            }
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            if (mainCamera != null)
            {
                mainCamera.transform.rotation = rotation;
                Debug.Log($"{GetType().Name}: Camera rotation set to {rotation}");
            }
            else
            {
                Debug.LogError($"{GetType().Name}: Cannot set rotation - no camera assigned!");
            }
        }

        public virtual void SetOffset(Vector3 newOffset)
        {
            offset = newOffset;
            Debug.Log($"{GetType().Name}: Camera offset set to {newOffset}");
        }

        public virtual void SetBounds(Vector2 min, Vector2 max)
        {
            minBounds = min;
            maxBounds = max;
            useBounds = true;
            Debug.Log($"{GetType().Name}: Camera bounds set to Min:{min}, Max:{max}");
        }

        public virtual void DisableBounds()
        {
            useBounds = false;
            Debug.Log($"{GetType().Name}: Camera bounds disabled");
        }

        public virtual Camera GetCamera()
        {
            return mainCamera;
        }

        public virtual void SetSmoothFollow(bool enabled, float speed = 5f)
        {
            smoothFollow = enabled;
            smoothSpeed = speed;
            Debug.Log($"{GetType().Name}: Smooth follow {(enabled ? "enabled" : "disabled")} with speed {speed}");
        }
    }
}
