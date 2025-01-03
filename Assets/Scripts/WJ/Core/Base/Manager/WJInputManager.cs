using UnityEngine;
using WJ.Core.Base.Input;

namespace WJ.Core.Base.Manager
{
    public class WJInputManager : MonoBehaviour
    {
        protected static WJInputManager instance;
        public static WJInputManager Instance => instance;

        protected WJInputInterface currentInput;

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

        public void SetInputSystem(WJInputInterface input)
        {
            currentInput = input;
        }

        public Vector2 GetMovementInput()
        {
            return currentInput?.GetMovementInput() ?? Vector2.zero;
        }

        public bool GetButtonDown(string buttonName)
        {
            return currentInput?.GetButtonDown(buttonName) ?? false;
        }

        public bool GetButton(string buttonName)
        {
            return currentInput?.GetButton(buttonName) ?? false;
        }

        public bool GetButtonUp(string buttonName)
        {
            return currentInput?.GetButtonUp(buttonName) ?? false;
        }
    }
} 