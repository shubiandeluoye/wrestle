using UnityEngine;
using System.Collections.Generic;

namespace WJ.Core.Base.Audio
{
    public class WJBaseAudioPool : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] protected int initialPoolSize = 5;
        [SerializeField] protected int maxPoolSize = 20;
        [SerializeField] protected GameObject audioSourcePrefab;

        protected Queue<WJBaseAudio> audioPool;
        protected List<WJBaseAudio> activeAudios;

        protected virtual void Awake()
        {
            audioPool = new Queue<WJBaseAudio>();
            activeAudios = new List<WJBaseAudio>();
            InitializePool();
        }

        protected virtual void InitializePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewAudioSource();
            }
        }

        protected virtual WJBaseAudio CreateNewAudioSource()
        {
            GameObject audioObj;
            if (audioSourcePrefab != null)
            {
                audioObj = Instantiate(audioSourcePrefab, transform);
            }
            else
            {
                audioObj = new GameObject("PooledAudio");
                audioObj.transform.SetParent(transform);
                audioObj.AddComponent<AudioSource>();
                audioObj.AddComponent<WJBaseAudio>();
            }

            WJBaseAudio audio = audioObj.GetComponent<WJBaseAudio>();
            audioObj.SetActive(false);
            audioPool.Enqueue(audio);
            return audio;
        }

        public virtual WJBaseAudio GetAudio()
        {
            if (audioPool.Count == 0 && activeAudios.Count < maxPoolSize)
            {
                CreateNewAudioSource();
            }

            WJBaseAudio audio = null;
            if (audioPool.Count > 0)
            {
                audio = audioPool.Dequeue();
                audio.gameObject.SetActive(true);
                activeAudios.Add(audio);
            }

            return audio;
        }

        public virtual void ReleaseAudio(WJBaseAudio audio)
        {
            if (audio != null && activeAudios.Contains(audio))
            {
                audio.Stop();
                audio.gameObject.SetActive(false);
                activeAudios.Remove(audio);
                audioPool.Enqueue(audio);
            }
        }

        public virtual void ReleaseAllAudio()
        {
            foreach (var audio in activeAudios.ToArray())
            {
                ReleaseAudio(audio);
            }
        }

        public virtual void PlayOneShot(AudioClip clip, Vector3 position, float volume = 1f)
        {
            WJBaseAudio audio = GetAudio();
            if (audio != null)
            {
                audio.transform.position = position;
                audio.SetVolume(volume);
                audio.SetClip(clip);
                audio.Play();

                // 在音频播放完成后自动回收
                StartCoroutine(AutoReleaseAudio(audio, clip.length));
            }
        }

        protected System.Collections.IEnumerator AutoReleaseAudio(WJBaseAudio audio, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReleaseAudio(audio);
        }
    }
} 