using UnityEngine;
using TMPro;

namespace WJ.Core.Base.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class WJBaseUIText : MonoBehaviour
    {
        protected TextMeshProUGUI textComponent;

        [Header("Text Settings")]
        [SerializeField] protected string defaultText = "";
        [SerializeField] protected bool updateOnStart = true;

        protected virtual void Awake()
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        protected virtual void Start()
        {
            if (updateOnStart)
            {
                SetText(defaultText);
            }
        }

        public virtual void SetText(string text)
        {
            if (textComponent != null)
            {
                textComponent.text = text;
            }
        }

        public virtual void SetText(int number)
        {
            SetText(number.ToString());
        }

        public virtual void SetText(float number)
        {
            SetText(number.ToString("F2"));
        }

        public virtual string GetText()
        {
            return textComponent != null ? textComponent.text : "";
        }

        public virtual void SetColor(Color color)
        {
            if (textComponent != null)
            {
                textComponent.color = color;
            }
        }

        public virtual void SetFontSize(float size)
        {
            if (textComponent != null)
            {
                textComponent.fontSize = size;
            }
        }
    }
} 