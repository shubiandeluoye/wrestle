using UnityEngine;

namespace Assets.Scripts.WJ.Core.Audio
{
    public class WJAudioManager : MonoBehaviour
    {
        public static WJAudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource shootSound;
        [SerializeField] private AudioSource hitSound;
        [SerializeField] private AudioSource bounceSound;
        [SerializeField] private AudioSource gameOverSound;

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
            if (shootSound != null) shootSound.Play();
        }

        public void PlayHitSound()
        {
            if (hitSound != null) hitSound.Play();
        }

        public void PlayBounceSound()
        {
            if (bounceSound != null) bounceSound.Play();
        }

        public void PlayGameOverSound()
        {
            if (gameOverSound != null) gameOverSound.Play();
        }
    }
} 