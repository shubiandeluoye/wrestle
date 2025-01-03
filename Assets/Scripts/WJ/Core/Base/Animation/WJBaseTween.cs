using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace WJ.Core.Base.Animation
{
    public class WJBaseTween : MonoBehaviour
    {
        [Header("Tween Settings")]
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] protected bool playOnStart = false;
        [SerializeField] protected bool loop = false;
        [SerializeField] protected bool pingPong = false;

        [Header("Events")]
        [SerializeField] protected UnityEvent onTweenStart;
        [SerializeField] protected UnityEvent onTweenComplete;

        protected bool isPlaying = false;
        protected float currentTime = 0f;
        protected bool isReverse = false;

        protected virtual void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        public virtual void Play()
        {
            currentTime = 0f;
            isPlaying = true;
            isReverse = false;
            onTweenStart?.Invoke();
            StartCoroutine(TweenCoroutine());
        }

        public virtual void PlayReverse()
        {
            currentTime = duration;
            isPlaying = true;
            isReverse = true;
            onTweenStart?.Invoke();
            StartCoroutine(TweenCoroutine());
        }

        public virtual void Stop()
        {
            isPlaying = false;
            StopAllCoroutines();
        }

        protected virtual IEnumerator TweenCoroutine()
        {
            do
            {
                while (isPlaying && (isReverse ? currentTime > 0 : currentTime < duration))
                {
                    currentTime += (isReverse ? -Time.deltaTime : Time.deltaTime);
                    float normalizedTime = Mathf.Clamp01(currentTime / duration);
                    float curveValue = curve.Evaluate(normalizedTime);
                    
                    UpdateTween(curveValue);
                    
                    yield return null;
                }

                if (pingPong)
                {
                    isReverse = !isReverse;
                }
                else if (loop)
                {
                    currentTime = isReverse ? duration : 0f;
                }
                else
                {
                    isPlaying = false;
                }

                if (!isPlaying)
                {
                    onTweenComplete?.Invoke();
                }

            } while (isPlaying && (loop || pingPong));
        }

        protected virtual void UpdateTween(float value)
        {
            // 在子类中实现具体的补间动画逻辑
        }
    }

    // 位置补间动画
    public class WJBasePositionTween : WJBaseTween
    {
        [Header("Position Settings")]
        [SerializeField] protected Vector3 startPosition;
        [SerializeField] protected Vector3 endPosition;

        protected override void UpdateTween(float value)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, value);
        }
    }

    // 缩放补间动画
    public class WJBaseScaleTween : WJBaseTween
    {
        [Header("Scale Settings")]
        [SerializeField] protected Vector3 startScale = Vector3.one;
        [SerializeField] protected Vector3 endScale = Vector3.one * 2f;

        protected override void UpdateTween(float value)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, value);
        }
    }

    // 旋转补间动画
    public class WJBaseRotationTween : WJBaseTween
    {
        [Header("Rotation Settings")]
        [SerializeField] protected Vector3 startRotation;
        [SerializeField] protected Vector3 endRotation;

        protected override void UpdateTween(float value)
        {
            transform.localRotation = Quaternion.Euler(
                Vector3.Lerp(startRotation, endRotation, value)
            );
        }
    }

    // 颜色补间动画
    public class WJBaseColorTween : WJBaseTween
    {
        [Header("Color Settings")]
        [SerializeField] protected Color startColor = Color.white;
        [SerializeField] protected Color endColor = Color.white;
        [SerializeField] protected UnityEngine.UI.Graphic target;

        protected override void UpdateTween(float value)
        {
            if (target != null)
            {
                target.color = Color.Lerp(startColor, endColor, value);
            }
        }
    }
} 