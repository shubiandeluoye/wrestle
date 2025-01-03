using UnityEngine.Events;

namespace WJ.Core.Base.Timer
{
    public interface IWJTimer
    {
        void StartTimer();
        void PauseTimer();
        void ResumeTimer();
        void StopTimer();
        float GetCurrentTime();
        float GetProgress();
        bool IsRunning();
    }
} 