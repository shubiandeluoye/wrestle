using UnityEngine;
using WJ.Core.Base.Input;

namespace WJ.Core.Base.Manager
{
    public class WJBaseInputManager : MonoBehaviour
    {
        protected static WJBaseInputManager instance;
        public static WJBaseInputManager Instance => instance;

        protected IWJInput currentInput;

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
            // 默认使用新输入系统
            currentInput = gameObject.AddComponent<WJBaseInput>();
        }

        public virtual void SetInputSystem(IWJInput inputSystem)
        {
            currentInput = inputSystem;
        }

        public virtual Vector2 GetMovementInput()
        {
            return currentInput?.GetMovementInput() ?? Vector2.zero;
        }

        public virtual bool GetButtonDown(string buttonName)
        {
            return currentInput?.GetButtonDown(buttonName) ?? false;
        }

        public virtual bool GetButton(string buttonName)
        {
            return currentInput?.GetButton(buttonName) ?? false;
        }

        public virtual bool GetButtonUp(string buttonName)
        {
            return currentInput?.GetButtonUp(buttonName) ?? false;
        }
    }
} 