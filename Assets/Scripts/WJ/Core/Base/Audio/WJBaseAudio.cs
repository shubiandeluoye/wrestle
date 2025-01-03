using UnityEngine;

namespace WJ.Core.Base.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class WJBaseAudio : MonoBehaviour
    {
        protected AudioSource audioSource;

        [Header("Audio Settings")]
        [SerializeField] protected bool playOnStart = false;
        [SerializeField] protected bool loop = false;
        [SerializeField] protected float defaultVolume = 1f;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = loop;
            audioSource.volume = defaultVolume;
        }

        protected virtual void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        public virtual void Play()
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }

        public virtual void Stop()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        public virtual void Pause()
        {
            if (audioSource != null)
            {
                audioSource.Pause();
            }
        }

        public virtual void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Clamp01(volume);
            }
        }

        public virtual void SetClip(AudioClip clip)
        {
            if (audioSource != null)
            {
                audioSource.clip = clip;
            }
        }
    }
} 