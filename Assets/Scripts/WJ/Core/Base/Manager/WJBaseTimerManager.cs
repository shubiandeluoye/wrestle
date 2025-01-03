using UnityEngine;
using System.Collections.Generic;
using WJ.Core.Base.Timer;

namespace WJ.Core.Base.Manager
{
    public class WJBaseTimerManager : MonoBehaviour
    {
        protected static WJBaseTimerManager instance;
        public static WJBaseTimerManager Instance => instance;

        protected Dictionary<string, IWJTimer> timers = new Dictionary<string, IWJTimer>();

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

        public virtual T CreateTimer<T>(string timerName) where T : Component, IWJTimer
        {
            if (timers.ContainsKey(timerName))
            {
                Debug.LogWarning($"Timer '{timerName}' already exists!");
                return null;
            }

            var timer = new GameObject($"Timer_{timerName}").AddComponent<T>();
            timer.transform.SetParent(transform);
            timers.Add(timerName, timer);
            return timer;
        }

        public virtual IWJTimer GetTimer(string timerName)
        {
            return timers.TryGetValue(timerName, out var timer) ? timer : null;
        }

        public virtual void RemoveTimer(string timerName)
        {
            if (timers.TryGetValue(timerName, out var timer))
            {
                if (timer is Component component)
                {
                    Destroy(component.gameObject);
                }
                timers.Remove(timerName);
            }
        }
    }
} 