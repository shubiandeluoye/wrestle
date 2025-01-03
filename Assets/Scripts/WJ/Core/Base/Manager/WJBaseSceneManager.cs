using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using WJ.Core.Base.UI;

namespace WJ.Core.Base.Manager.Scene
{
    public class WJBaseSceneManager : MonoBehaviour
    {
        protected static WJBaseSceneManager instance;
        public static WJBaseSceneManager Instance => instance;

        [Header("Scene Settings")]
        [SerializeField] protected string loadingSceneName = "LoadingScene";
        [SerializeField] protected float minimumLoadingTime = 0.5f;
        [SerializeField] protected WJBaseUI loadingUI;

        protected string currentSceneName;
        protected string targetSceneName;
        protected bool isLoading;

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
            }
        }

        public virtual void LoadScene(string sceneName)
        {
            if (!isLoading && sceneName != currentSceneName)
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            }
        }

        protected virtual IEnumerator LoadSceneAsync(string sceneName)
        {
            isLoading = true;
            targetSceneName = sceneName;

            // 显示加载界面
            if (loadingUI != null)
            {
                loadingUI.Show();
            }

            // 记录开始时间
            float startTime = Time.time;

            // 加载场景
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                float progress = operation.progress / 0.9f;
                UpdateLoadingProgress(progress);
                yield return null;
            }

            // 确保最小加载时间
            float elapsedTime = Time.time - startTime;
            if (elapsedTime < minimumLoadingTime)
            {
                yield return new WaitForSeconds(minimumLoadingTime - elapsedTime);
            }

            // 完成加载
            UpdateLoadingProgress(1);
            operation.allowSceneActivation = true;
            while (!operation.isDone)
            {
                yield return null;
            }

            // 隐藏加载界面
            if (loadingUI != null)
            {
                loadingUI.Hide();
            }

            currentSceneName = targetSceneName;
            isLoading = false;
        }

        protected virtual void UpdateLoadingProgress(float progress)
        {
            // 可以在这里更新加载进度UI
        }

        public virtual string GetCurrentSceneName()
        {
            return currentSceneName;
        }

        public virtual bool IsLoading()
        {
            return isLoading;
        }

        public virtual void ReloadCurrentScene()
        {
            if (currentSceneName != null)
            {
                LoadScene(currentSceneName);
            }
        }
    }
} 