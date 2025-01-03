using UnityEngine;
using UnityEngine.UI;

namespace WJ.Core.Base.UI
{
    public class WJBaseUIProgressBar : MonoBehaviour
    {
        [Header("Progress Bar References")]
        [SerializeField] protected Image fillImage;
        [SerializeField] protected WJBaseUIText valueText;

        [Header("Progress Bar Settings")]
        [SerializeField] protected bool showValue = true;
        [SerializeField] protected string valueFormat = "{0:0}%";
        [SerializeField] protected float smoothSpeed = 5f;
        
        protected float currentValue = 1f;
        protected float targetValue = 1f;

        protected virtual void Start()
        {
            UpdateUI();
        }

        protected virtual void Update()
        {
            if (!Mathf.Approximately(currentValue, targetValue))
            {
                currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * smoothSpeed);
                UpdateUI();
            }
        }

        public virtual void SetValue(float value, float maxValue)
        {
            targetValue = Mathf.Clamp01(value / maxValue);
        }

        public virtual void SetValueImmediate(float value, float maxValue)
        {
            targetValue = currentValue = Mathf.Clamp01(value / maxValue);
            UpdateUI();
        }

        protected virtual void UpdateUI()
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = currentValue;
            }

            if (showValue && valueText != null)
            {
                valueText.SetText(string.Format(valueFormat, currentValue * 100));
            }
        }

        public virtual void SetColor(Color color)
        {
            if (fillImage != null)
            {
                fillImage.color = color;
            }
        }
    }
} 