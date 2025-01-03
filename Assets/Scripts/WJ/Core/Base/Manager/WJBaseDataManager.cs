using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace WJ.Core.Base.Manager.Data
{
    public class WJBaseDataManager : MonoBehaviour
    {
        protected static WJBaseDataManager instance;
        public static WJBaseDataManager Instance => instance;

        [Header("Data Settings")]
        [SerializeField] protected string defaultSaveFileName = "gameData.json";
        [SerializeField] protected bool useEncryption = false;
        [SerializeField] protected bool autoSave = true;
        [SerializeField] protected float autoSaveInterval = 300f; // 5分钟

        protected float nextAutoSaveTime;
        protected Dictionary<string, object> gameData;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Start()
        {
            LoadData();
            if (autoSave)
            {
                nextAutoSaveTime = Time.time + autoSaveInterval;
            }
        }

        protected virtual void Update()
        {
            if (autoSave && Time.time >= nextAutoSaveTime)
            {
                SaveData();
                nextAutoSaveTime = Time.time + autoSaveInterval;
            }
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveData();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            SaveData();
        }

        protected virtual void InitializeData()
        {
            gameData = new Dictionary<string, object>();
        }

        public virtual void SaveData()
        {
            string json = JsonUtility.ToJson(new SerializableDict<string, object>(gameData));
            
            if (useEncryption)
            {
                json = EncryptData(json);
            }

            string path = Path.Combine(Application.persistentDataPath, defaultSaveFileName);
            File.WriteAllText(path, json);
        }

        public virtual void LoadData()
        {
            string path = Path.Combine(Application.persistentDataPath, defaultSaveFileName);
            
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                
                if (useEncryption)
                {
                    json = DecryptData(json);
                }

                SerializableDict<string, object> data = JsonUtility.FromJson<SerializableDict<string, object>>(json);
                gameData = data.ToDictionary();
            }
        }

        public virtual void SetData<T>(string key, T value)
        {
            gameData[key] = value;
        }

        public virtual T GetData<T>(string key, T defaultValue = default)
        {
            if (gameData.TryGetValue(key, out object value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }
            }
            return defaultValue;
        }

        public virtual void DeleteData(string key)
        {
            if (gameData.ContainsKey(key))
            {
                gameData.Remove(key);
            }
        }

        public virtual void ClearAllData()
        {
            gameData.Clear();
            SaveData();
        }

        protected virtual string EncryptData(string data)
        {
            // 简单的加密示例，实际使用时应该使用更安全的加密方法
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
        }

        protected virtual string DecryptData(string encryptedData)
        {
            // 简单的解密示例
            byte[] bytes = System.Convert.FromBase64String(encryptedData);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }

    [System.Serializable]
    public class SerializableDict<TKey, TValue>
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        public SerializableDict() { }

        public SerializableDict(Dictionary<TKey, TValue> dict)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++)
            {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }
    }
} 