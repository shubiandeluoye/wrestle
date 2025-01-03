using UnityEngine;
using System.Collections.Generic;

namespace WJ.Core.Base.Manager
{
    public class WJBaseAnimationManager : MonoBehaviour
    {
        protected static WJBaseAnimationManager instance;
        public static WJBaseAnimationManager Instance => instance;

        [Header("Animation Settings")]
        [SerializeField] protected RuntimeAnimatorController defaultController;
        
        protected Dictionary<string, RuntimeAnimatorController> controllerCache;

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
            controllerCache = new Dictionary<string, RuntimeAnimatorController>();
        }

        public virtual RuntimeAnimatorController LoadAnimatorController(string path)
        {
            if (controllerCache.TryGetValue(path, out RuntimeAnimatorController controller))
            {
                return controller;
            }

            controller = Resources.Load<RuntimeAnimatorController>(path);
            if (controller != null)
            {
                controllerCache[path] = controller;
            }

            return controller;
        }

        public virtual void SetAnimatorController(Animator animator, string controllerPath)
        {
            if (animator != null)
            {
                RuntimeAnimatorController controller = LoadAnimatorController(controllerPath);
                if (controller != null)
                {
                    animator.runtimeAnimatorController = controller;
                }
            }
        }

        public virtual void ClearCache()
        {
            controllerCache.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
} 