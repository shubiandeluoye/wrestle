using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace WJ.Core.Base.Manager
{
    public class WJBaseUIManager : MonoBehaviour
    {
        protected static WJBaseUIManager instance;
        public static WJBaseUIManager Instance => instance;

        // 资源缓存
        protected Dictionary<string, Object> resourceCache;
        
        [Header("Resource Settings")]
        [SerializeField] protected bool useCache = true;
        [SerializeField] protected int maxCacheSize = 100;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void InitializeManager()
        {
            resourceCache = new Dictionary<string, Object>();
        }

        // 同步加载资源
        public virtual T LoadResource<T>(string path) where T : Object
        {
            // 检查缓存
            if (useCache && resourceCache.TryGetValue(path, out Object cachedResource))
            {
                return cachedResource as T;
            }

            // 加载资源
            T resource = Resources.Load<T>(path);
            
            // 缓存资源
            if (useCache && resource != null)
            {
                CacheResource(path, resource);
            }

            return resource;
        }

        // 异步加载资源
        public virtual IEnumerator LoadResourceAsync<T>(string path, System.Action<T> callback) where T : Object
        {
            // 检查缓存
            if (useCache && resourceCache.TryGetValue(path, out Object cachedResource))
            {
                callback?.Invoke(cachedResource as T);
                yield break;
            }

            // 异步加载
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;

            T resource = request.asset as T;
            
            // 缓存资源
            if (useCache && resource != null)
            {
                CacheResource(path, resource);
            }

            callback?.Invoke(resource);
        }

        // 缓存资源
        protected virtual void CacheResource(string path, Object resource)
        {
            // 检查缓存大小
            if (resourceCache.Count >= maxCacheSize)
            {
                // 简单的缓存清理策略：清除第一个
                var enumerator = resourceCache.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    resourceCache.Remove(enumerator.Current.Key);
                }
            }

            resourceCache[path] = resource;
        }

        // 从缓存中移除资源
        public virtual void RemoveFromCache(string path)
        {
            if (resourceCache.ContainsKey(path))
            {
                resourceCache.Remove(path);
            }
        }

        // 清除所有缓存
        public virtual void ClearCache()
        {
            resourceCache.Clear();
            Resources.UnloadUnusedAssets();
        }

        // 预加载资源
        public virtual void PreloadResources(string[] paths)
        {
            foreach (string path in paths)
            {
                LoadResource<Object>(path);
            }
        }

        // 异步预加载资源
        public virtual IEnumerator PreloadResourcesAsync(string[] paths, System.Action<float> progressCallback = null)
        {
            float totalCount = paths.Length;
            float currentCount = 0;

            foreach (string path in paths)
            {
                yield return LoadResourceAsync<Object>(path, (resource) => {
                    currentCount++;
                    progressCallback?.Invoke(currentCount / totalCount);
                });
            }
        }

        protected virtual void OnDestroy()
        {
            ClearCache();
        }
    }
} 