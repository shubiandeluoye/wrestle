using UnityEngine;
using System.Collections.Generic;

namespace WJ.Core.Base.UI
{
    public class WJBaseUIManager : MonoBehaviour
    {
        protected static WJBaseUIManager instance;
        public static WJBaseUIManager Instance => instance;

        protected Dictionary<string, WJBaseUI> uiDictionary = new Dictionary<string, WJBaseUI>();

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

        public virtual void RegisterUI(string uiName, WJBaseUI ui)
        {
            if (!uiDictionary.ContainsKey(uiName))
            {
                uiDictionary.Add(uiName, ui);
            }
        }

        public virtual void UnregisterUI(string uiName)
        {
            if (uiDictionary.ContainsKey(uiName))
            {
                uiDictionary.Remove(uiName);
            }
        }

        public virtual void ShowUI(string uiName)
        {
            if (uiDictionary.TryGetValue(uiName, out WJBaseUI ui))
            {
                ui.Show();
            }
        }

        public virtual void HideUI(string uiName)
        {
            if (uiDictionary.TryGetValue(uiName, out WJBaseUI ui))
            {
                ui.Hide();
            }
        }

        public virtual WJBaseUI GetUI(string uiName)
        {
            if (uiDictionary.TryGetValue(uiName, out WJBaseUI ui))
            {
                return ui;
            }
            return null;
        }
    }
} 