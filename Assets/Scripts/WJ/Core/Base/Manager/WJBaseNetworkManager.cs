using UnityEngine;
using WJ.Core.Network.Base;
using System;
using System.Collections.Generic;

namespace WJ.Core.Base.Manager
{
    public abstract class WJBaseNetworkManager : MonoBehaviour
    {
        protected static WJBaseNetworkManager instance;
        public static WJBaseNetworkManager Instance => instance;

        [Header("Network Settings")]
        [SerializeField] protected WJNetworkPrefabsRef prefabsRef;
        [SerializeField] protected bool autoConnect = true;
        [SerializeField] protected string defaultRoom = "DefaultRoom";
        [SerializeField] protected float reconnectDelay = 5f;
        [SerializeField] protected int maxReconnectAttempts = 3;

        [Header("Network Status")]
        [SerializeField] protected bool isConnected;
        [SerializeField] protected bool isHost;
        [SerializeField] protected string currentRoom;

        protected int reconnectAttempts;
        protected Dictionary<string, Action<object>> networkCallbacks;

        public bool IsConnected => isConnected;
        public bool IsHost => isHost;
        public string CurrentRoom => currentRoom;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeNetwork();
            }
            else
            {
                Debug.LogWarning($"{GetType().Name}: Multiple instances detected. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        protected virtual void Start()
        {
            if (autoConnect)
            {
                Connect();
            }
        }

        protected virtual void InitializeNetwork()
        {
            networkCallbacks = new Dictionary<string, Action<object>>();
            
            if (prefabsRef == null)
            {
                Debug.LogError($"{GetType().Name}: Network prefabs reference is missing!");
                return;
            }

            ValidateNetworkPrefabs();
        }

        protected virtual void ValidateNetworkPrefabs()
        {
            if (prefabsRef != null)
            {
                prefabsRef.ValidateReferences();
            }
        }

        public virtual void Connect()
        {
            if (isConnected)
            {
                Debug.LogWarning($"{GetType().Name}: Already connected!");
                return;
            }

            Debug.Log($"{GetType().Name}: Attempting to connect...");
            // Implementation specific connection logic
        }

        public virtual void Disconnect()
        {
            if (!isConnected)
            {
                Debug.LogWarning($"{GetType().Name}: Not connected!");
                return;
            }

            Debug.Log($"{GetType().Name}: Disconnecting from network...");
            // Implementation specific disconnection logic
        }

        public virtual void JoinRoom(string roomName = null)
        {
            if (!isConnected)
            {
                Debug.LogError($"{GetType().Name}: Cannot join room - not connected!");
                return;
            }

            string targetRoom = roomName ?? defaultRoom;
            Debug.Log($"{GetType().Name}: Attempting to join room: {targetRoom}");
            // Implementation specific room joining logic
        }

        public virtual void LeaveRoom()
        {
            if (string.IsNullOrEmpty(currentRoom))
            {
                Debug.LogWarning($"{GetType().Name}: Not in any room!");
                return;
            }

            Debug.Log($"{GetType().Name}: Leaving room: {currentRoom}");
            // Implementation specific room leaving logic
        }

        public virtual void RegisterCallback(string eventName, Action<object> callback)
        {
            if (!networkCallbacks.ContainsKey(eventName))
            {
                networkCallbacks[eventName] = callback;
            }
            else
            {
                networkCallbacks[eventName] += callback;
            }
        }

        public virtual void UnregisterCallback(string eventName, Action<object> callback)
        {
            if (networkCallbacks.ContainsKey(eventName))
            {
                networkCallbacks[eventName] -= callback;
                if (networkCallbacks[eventName] == null)
                {
                    networkCallbacks.Remove(eventName);
                }
            }
        }

        protected virtual void OnNetworkEvent(string eventName, object data)
        {
            if (networkCallbacks.TryGetValue(eventName, out Action<object> callback))
            {
                callback?.Invoke(data);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (isConnected)
            {
                Disconnect();
            }
        }

        public virtual void SetAutoReconnect(bool enabled, float delay = 5f, int maxAttempts = 3)
        {
            autoConnect = enabled;
            reconnectDelay = delay;
            maxReconnectAttempts = maxAttempts;
            Debug.Log($"{GetType().Name}: Auto reconnect {(enabled ? "enabled" : "disabled")} with {maxAttempts} attempts every {delay} seconds");
        }
    }
}
