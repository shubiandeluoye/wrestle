using UnityEngine;

namespace Assets.Scripts.WJ.Core.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class WJAudioManager : MonoBehaviour
    {
        public static WJAudioManager Instance { get; private set; }

        [Header("Audio Clips")]
        [SerializeField] private AudioClip shootSound;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip bounceSound;
        [SerializeField] private AudioClip gameOverSound;

        private AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                audioSource = GetComponent<AudioSource>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayShootSound()
        {
            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }

        public void PlayHitSound()
        {
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }

        public void PlayBounceSound()
        {
            if (bounceSound != null)
            {
                audioSource.PlayOneShot(bounceSound);
            }
        }

        public void PlayGameOverSound()
        {
            if (gameOverSound != null)
            {
                audioSource.PlayOneShot(gameOverSound);
            }
        }
    }
} 