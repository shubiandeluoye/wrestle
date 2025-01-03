using UnityEngine;
using System;
using System.Collections.Generic;

namespace WJ.Core.Base.Manager
{
    public class WJBaseEventManager : MonoBehaviour
    {
        protected static WJBaseEventManager instance;
        public static WJBaseEventManager Instance => instance;

        // 事件字典，存储所有事件及其监听者
        protected Dictionary<string, Action<object[]>> eventDictionary;

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
            eventDictionary = new Dictionary<string, Action<object[]>>();
        }

        // 添加事件监听
        public virtual void AddListener(string eventName, Action<object[]> listener)
        {
            if (eventDictionary.TryGetValue(eventName, out Action<object[]> thisEvent))
            {
                thisEvent += listener;
                eventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent = listener;
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        // 移除事件监听
        public virtual void RemoveListener(string eventName, Action<object[]> listener)
        {
            if (eventDictionary.TryGetValue(eventName, out Action<object[]> thisEvent))
            {
                thisEvent -= listener;
                if (thisEvent == null)
                {
                    eventDictionary.Remove(eventName);
                }
                else
                {
                    eventDictionary[eventName] = thisEvent;
                }
            }
        }

        // 触发事件
        public virtual void TriggerEvent(string eventName, params object[] parameters)
        {
            if (eventDictionary.TryGetValue(eventName, out Action<object[]> thisEvent))
            {
                thisEvent.Invoke(parameters);
            }
        }

        // 清除所有事件
        public virtual void ClearAllEvents()
        {
            eventDictionary.Clear();
        }

        // 清除特定事件
        public virtual void ClearEvent(string eventName)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary.Remove(eventName);
            }
        }

        // 检查事件是否存在
        public virtual bool HasEvent(string eventName)
        {
            return eventDictionary.ContainsKey(eventName);
        }

        // 获取事件监听者数量
        public virtual int GetListenerCount(string eventName)
        {
            if (eventDictionary.TryGetValue(eventName, out Action<object[]> thisEvent))
            {
                return thisEvent.GetInvocationList().Length;
            }
            return 0;
        }

        protected virtual void OnDestroy()
        {
            ClearAllEvents();
        }
    }
}