using UnityEngine;
using UnityEngine.Events;

namespace WJ.Core.Base.Timer
{
    public class WJBaseTimer : MonoBehaviour, IWJTimer
    {
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected bool countDown = true;
        
        public UnityEvent<float> onTimerUpdate = new UnityEvent<float>();
        public UnityEvent onTimerComplete = new UnityEvent();

        protected float currentTime;
        protected bool isRunning;
        protected bool isPaused;

        public virtual void StartTimer()
        {
            currentTime = countDown ? duration : 0f;
            isRunning = true;
            isPaused = false;
        }

        protected virtual void Update()
        {
            if (!isRunning || isPaused) return;

            if (countDown)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0f)
                {
                    CompleteTimer();
                }
            }
            else
            {
                currentTime += Time.deltaTime;
                if (currentTime >= duration)
                {
                    CompleteTimer();
                }
            }

            onTimerUpdate?.Invoke(currentTime);
        }

        protected virtual void CompleteTimer()
        {
            currentTime = countDown ? 0f : duration;
            isRunning = false;
            onTimerComplete?.Invoke();
        }

        public virtual void PauseTimer() => isPaused = true;
        public virtual void ResumeTimer() => isPaused = false;
        public virtual void StopTimer() 
        {
            isRunning = false;
            isPaused = false;
        }

        public virtual float GetCurrentTime() => currentTime;
        public virtual float GetProgress() => countDown ? 
            1f - (currentTime / duration) : currentTime / duration;
        public virtual bool IsRunning() => isRunning && !isPaused;
    }
} 