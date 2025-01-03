using UnityEngine;
using System.Collections.Generic;

namespace WJ.Core.Base.Audio
{
    public class WJBaseAudioManager : MonoBehaviour
    {
        protected static WJBaseAudioManager instance;
        public static WJBaseAudioManager Instance => instance;

        [Header("Audio Sources")]
        [SerializeField] protected AudioSource musicSource;
        [SerializeField] protected AudioSource sfxSource;

        [Header("Volume Settings")]
        [SerializeField] protected float masterVolume = 1f;
        [SerializeField] protected float musicVolume = 1f;
        [SerializeField] protected float sfxVolume = 1f;

        protected Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void InitializeAudioSources()
        {
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public virtual void PlayMusic(AudioClip clip)
        {
            if (musicSource != null && clip != null)
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }

        public virtual void PlaySFX(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }

        public virtual void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        public virtual void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        public virtual void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        protected virtual void UpdateVolumes()
        {
            if (musicSource != null)
                musicSource.volume = masterVolume * musicVolume;

            if (sfxSource != null)
                sfxSource.volume = masterVolume * sfxVolume;
        }

        public virtual void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        public virtual void PauseMusic()
        {
            if (musicSource != null)
            {
                musicSource.Pause();
            }
        }

        public virtual void ResumeMusic()
        {
            if (musicSource != null)
            {
                musicSource.UnPause();
            }
        }
    }
} 