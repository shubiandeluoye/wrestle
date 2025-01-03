using UnityEngine;

public class WJBulletAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip bounceSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;  // 2D音效
            audioSource.volume = 1f;
        }
    }

    public void PlayShootSound()
    {
        PlaySound(shootSound);
    }

    public void PlayHitSound()
    {
        PlaySound(hitSound);
    }

    public void PlayBounceSound()
    {
        PlaySound(bounceSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
} 