using Data;
using System;
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

        public static void SaveData<T>(T data, string key, string extraOptions)
        {
            string json = ToJson(data);
            PlayerPrefs.SetString(key + extraOptions, json);
            PlayerPrefs.Save();
        }

        public static void ClearRestoreData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}