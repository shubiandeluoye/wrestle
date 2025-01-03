using UnityEngine;

namespace WJ.Core.Base.Manager
{
    public class WJBaseResourceManager : MonoBehaviour
    {
        protected static WJBaseResourceManager instance;
        public static WJBaseResourceManager Instance => instance;

        [Header("Game Settings")]
        [SerializeField] protected bool gameStarted;
        [SerializeField] protected bool gamePaused;
        
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

        public virtual void StartGame()
        {
            if (gameStarted) return;
            gameStarted = true;
        }

        public virtual void PauseGame()
        {
            gamePaused = true;
            Time.timeScale = 0;
        }

        public virtual void ResumeGame()
        {
            gamePaused = false;
            Time.timeScale = 1;
        }

        public virtual void EndGame()
        {
            gameStarted = false;
        }

        public virtual bool IsGameStarted()
        {
            return gameStarted;
        }

        public virtual bool IsGamePaused()
        {
            return gamePaused;
        }
    }
} 