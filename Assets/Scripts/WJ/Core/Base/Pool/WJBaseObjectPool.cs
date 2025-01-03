using UnityEngine;
using System.Collections.Generic;

namespace WJ.Core.Base.Pool
{
    public class WJBaseObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [Header("Pool Settings")]
        [SerializeField] protected List<Pool> pools;
        
        protected Dictionary<string, Queue<GameObject>> poolDictionary;

        protected static WJBaseObjectPool instance;
        public static WJBaseObjectPool Instance => instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            InitializePools();
        }

        protected virtual void InitializePools()
        {
            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = CreateNewObject(pool.prefab);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        protected virtual GameObject CreateNewObject(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            return obj;
        }

        public virtual GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }

            Queue<GameObject> pool = poolDictionary[tag];

            // 如果池为空，创建新对象
            if (pool.Count == 0)
            {
                Pool poolSettings = pools.Find(p => p.tag == tag);
                if (poolSettings != null)
                {
                    GameObject newObj = CreateNewObject(poolSettings.prefab);
                    pool.Enqueue(newObj);
                }
            }

            GameObject objectToSpawn = pool.Dequeue();

            if (objectToSpawn != null)
            {
                objectToSpawn.SetActive(true);
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;

                IPoolable poolable = objectToSpawn.GetComponent<IPoolable>();
                if (poolable != null)
                {
                    poolable.OnSpawnFromPool();
                }
            }

            return objectToSpawn;
        }

        public virtual void ReturnToPool(string tag, GameObject obj)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return;
            }

            IPoolable poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.OnReturnToPool();
            }

            obj.SetActive(false);
            obj.transform.SetParent(transform);
            poolDictionary[tag].Enqueue(obj);
        }
    }

    // 可选的接口，用于对象池中的对象
    public interface IPoolable
    {
        void OnSpawnFromPool();
        void OnReturnToPool();
    }
} 