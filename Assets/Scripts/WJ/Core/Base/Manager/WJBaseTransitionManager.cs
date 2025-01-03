using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace WJ.Core.Base.Manager
{
    public abstract class WJBaseTransitionManager : MonoBehaviour
    {
        protected static WJBaseTransitionManager instance;
        public static WJBaseTransitionManager Instance => instance;

        [Header("Transition Settings")]
        [SerializeField] protected float transitionDuration = 1f;
        [SerializeField] protected AnimationCurve transitionCurve;
        [SerializeField] protected CanvasGroup transitionPanel;
        [SerializeField] protected bool useLoadingScreen = true;
        [SerializeField] protected string loadingSceneName = "Loading";

        protected bool isTransitioning;
        public bool IsTransitioning => isTransitioning;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeTransitionManager();
            }
            else
            {
                Debug.LogWarning($"{GetType().Name}: Multiple instances detected. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        protected virtual void InitializeTransitionManager()
        {
            if (transitionPanel == null)
            {
                Debug.LogError($"{GetType().Name}: Transition panel is not assigned!");
            }

            if (transitionCurve == null)
            {
                transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
                Debug.LogWarning($"{GetType().Name}: No transition curve assigned, using default ease in/out curve.");
            }
        }

        public virtual void PlayTransition(string targetScene, System.Action onTransitionComplete = null)
        {
            if (isTransitioning)
            {
                Debug.LogWarning($"{GetType().Name}: Transition already in progress!");
                return;
            }

            if (string.IsNullOrEmpty(targetScene))
            {
                Debug.LogError($"{GetType().Name}: Target scene name is null or empty!");
                return;
            }

            StartCoroutine(TransitionCoroutine(targetScene, onTransitionComplete));
        }

        protected virtual IEnumerator TransitionCoroutine(string targetScene, System.Action onTransitionComplete)
        {
            isTransitioning = true;

            // Fade out
            yield return StartCoroutine(FadeCoroutine(0f, 1f));

            // Load loading screen if enabled
            if (useLoadingScreen)
            {
                yield return SceneManager.LoadSceneAsync(loadingSceneName);
                OnLoadingSceneLoaded();
            }

            // Load target scene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                OnLoadingProgressUpdated(asyncLoad.progress);
                yield return null;
            }

            // Wait for any additional loading tasks
            yield return StartCoroutine(OnBeforeSceneActivation());

            asyncLoad.allowSceneActivation = true;
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Notify scene loaded
            OnSceneLoaded(targetScene);

            // Fade in
            yield return StartCoroutine(FadeCoroutine(1f, 0f));

            isTransitioning = false;
            onTransitionComplete?.Invoke();
        }

        protected virtual IEnumerator FadeCoroutine(float startAlpha, float targetAlpha)
        {
            if (transitionPanel == null) yield break;

            float elapsedTime = 0f;
            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / transitionDuration;
                float curveValue = transitionCurve.Evaluate(normalizedTime);
                float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, curveValue);

                transitionPanel.alpha = currentAlpha;
                yield return null;
            }

            transitionPanel.alpha = targetAlpha;
        }

        protected virtual void OnLoadingSceneLoaded()
        {
            Debug.Log($"{GetType().Name}: Loading scene loaded.");
        }

        protected virtual void OnLoadingProgressUpdated(float progress)
        {
            Debug.Log($"{GetType().Name}: Loading progress: {progress:P0}");
        }

        protected virtual void OnSceneLoaded(string sceneName)
        {
            Debug.Log($"{GetType().Name}: Scene loaded: {sceneName}");
        }

        protected virtual IEnumerator OnBeforeSceneActivation()
        {
            yield return null;
        }

        public virtual void SetTransitionDuration(float duration)
        {
            if (duration < 0)
            {
                Debug.LogError($"{GetType().Name}: Invalid transition duration: {duration}. Must be greater than 0.");
                return;
            }

            transitionDuration = duration;
            Debug.Log($"{GetType().Name}: Transition duration set to {duration} seconds.");
        }


        public virtual void SetTransitionCurve(AnimationCurve curve)
        {
            if (curve == null)
            {
                Debug.LogError($"{GetType().Name}: Cannot set null transition curve!");
                return;
            }

            transitionCurve = curve;
            Debug.Log($"{GetType().Name}: Transition curve updated.");
        }

        public virtual void SetUseLoadingScreen(bool use)
        {
            useLoadingScreen = use;
            Debug.Log($"{GetType().Name}: Loading screen {(use ? "enabled" : "disabled")}.");
        }
    }
}
