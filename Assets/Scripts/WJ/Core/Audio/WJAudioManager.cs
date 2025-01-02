using UnityEngine;

namespace Assets.Scripts.WJ.Core.Audio
{
    public class WJAudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip shootSound;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip bounceSound;
        [SerializeField] private AudioClip gameOverSound;

        public static WJAudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayShootSound()
        {
            if (shootSound) sfxSource.PlayOneShot(shootSound);
        }

        public void PlayHitSound()
        {
            if (hitSound) sfxSource.PlayOneShot(hitSound);
        }

        public void PlayBounceSound()
        {
            if (bounceSound) sfxSource.PlayOneShot(bounceSound);
        }

        public void PlayGameOverSound()
        {
            if (gameOverSound) sfxSource.PlayOneShot(gameOverSound);
        }
    }
} 