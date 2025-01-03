using UnityEngine;
using System.Collections;

namespace WJ.Core.Base.Animation
{
    public class WJBaseAnimation : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected bool playOnStart = false;
        [SerializeField] protected string defaultAnimationName = "Idle";

        protected virtual void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        protected virtual void Start()
        {
            if (playOnStart)
            {
                PlayAnimation(defaultAnimationName);
            }
        }

        public virtual void PlayAnimation(string animationName)
        {
            if (animator != null)
            {
                animator.Play(animationName);
            }
        }

        public virtual void SetTrigger(string triggerName)
        {
            if (animator != null)
            {
                animator.SetTrigger(triggerName);
            }
        }

        public virtual void SetBool(string paramName, bool value)
        {
            if (animator != null)
            {
                animator.SetBool(paramName, value);
            }
        }

        public virtual void SetFloat(string paramName, float value)
        {
            if (animator != null)
            {
                animator.SetFloat(paramName, value);
            }
        }

        public virtual void SetInteger(string paramName, int value)
        {
            if (animator != null)
            {
                animator.SetInteger(paramName, value);
            }
        }

        public virtual float GetAnimationLength(string animationName)
        {
            if (animator != null)
            {
                AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                foreach (AnimatorClipInfo clip in clipInfo)
                {
                    if (clip.clip.name == animationName)
                    {
                        return clip.clip.length;
                    }
                }
            }
            return 0f;
        }

        public virtual bool IsPlaying(string animationName)
        {
            if (animator != null)
            {
                return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
            }
            return false;
        }
    }
} 