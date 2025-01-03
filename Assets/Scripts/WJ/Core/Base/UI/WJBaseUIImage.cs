using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace WJ.Core.Base.UI
{
    [RequireComponent(typeof(Image))]
    public class WJBaseUIImage : MonoBehaviour
    {
        protected Image image;
        
        [Header("Image Settings")]
        [SerializeField] protected bool fadeOnEnable = false;
        [SerializeField] protected float fadeDuration = 0.5f;

        protected virtual void Awake()
        {
            image = GetComponent<Image>();
        }

        protected virtual void OnEnable()
        {
            if (fadeOnEnable)
                StartCoroutine(FadeIn());
        }

        public virtual void SetSprite(Sprite sprite)
        {
            if (image != null)
                image.sprite = sprite;
        }

        public virtual void SetColor(Color color)
        {
            if (image != null)
                image.color = color;
        }

        public virtual IEnumerator FadeIn()
        {
            if (image != null)
            {
                Color color = image.color;
                color.a = 0;
                image.color = color;

                while (color.a < 1)
                {
                    color.a += Time.deltaTime / fadeDuration;
                    image.color = color;
                    yield return null;
                }
            }
        }

        public virtual IEnumerator FadeOut()
        {
            if (image != null)
            {
                Color color = image.color;
                while (color.a > 0)
                {
                    color.a -= Time.deltaTime / fadeDuration;
                    image.color = color;
                    yield return null;
                }
            }
        }
    }
} 