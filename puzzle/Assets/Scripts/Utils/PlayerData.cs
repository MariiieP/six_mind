using UnityEngine;
using static UnityEngine.JsonUtility;

namespace Utils
{
    public static class PlayerData
    {
        public static T LoadData<T>(string key, string extraOptions)
        {
            var extraKey = key + extraOptions;
            if (PlayerPrefs.HasKey(extraKey))
            {
                string json = PlayerPrefs.GetString(extraKey);
                return FromJson<T>(json);
            }
            return default;
        }

        public static int LoadInt(string key, string extraOptions)
        {
            var extraKey = key + extraOptions;
            int result = 0;
            if (PlayerPrefs.HasKey(extraKey))
            {
                result = PlayerPrefs.GetInt(extraKey);
            }
            return result;
        }

        public static void SaveData<T>(T data, string key, string extraOptions)
        {
            string json = ToJson(data);
            PlayerPrefs.SetString(key + extraOptions, json);
            PlayerPrefs.Save();
        }

        public static void SaveInt(int data, string key, string extraOptions)
        {
            PlayerPrefs.SetInt(key + extraOptions, data);
            PlayerPrefs.Save();
        }

        public static void ClearRestoreData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}