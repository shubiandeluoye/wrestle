using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace WJ.Core.Base.UI
{
    public class WJBaseUIScrollList : MonoBehaviour
    {
        [Header("Scroll List Settings")]
        [SerializeField] protected ScrollRect scrollRect;
        [SerializeField] protected RectTransform content;
        [SerializeField] protected GameObject itemPrefab;
        
        protected List<GameObject> items = new List<GameObject>();

        protected virtual void Awake()
        {
            if (scrollRect == null)
                scrollRect = GetComponent<ScrollRect>();
                
            if (content == null && scrollRect != null)
                content = scrollRect.content;
        }

        public virtual void AddItem(GameObject item)
        {
            if (content != null)
            {
                item.transform.SetParent(content, false);
                items.Add(item);
            }
        }

        public virtual GameObject CreateItem()
        {
            if (itemPrefab != null && content != null)
            {
                GameObject item = Instantiate(itemPrefab, content);
                items.Add(item);
                return item;
            }
            return null;
        }

        public virtual void ClearItems()
        {
            foreach (var item in items)
            {
                if (item != null)
                    Destroy(item);
            }
            items.Clear();
        }

        public virtual void ScrollToTop()
        {
            if (scrollRect != null)
                scrollRect.normalizedPosition = new Vector2(0, 1);
        }

        public virtual void ScrollToBottom()
        {
            if (scrollRect != null)
                scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
} 