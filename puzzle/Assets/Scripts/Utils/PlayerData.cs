using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using static UnityEngine.JsonUtility;

namespace Utils
{
    public static class PlayerData
    {
        private static readonly string _sessionKey = "SessionKey";
        private static readonly string _restoreKey = "RestoreKey";
        

        [Serializable]
        public class RestoreData
        {
            [Serializable]
            public struct LetterPart 
            {
                public Vector3 Rotation;
                public Vector3 Position;
            }
    
            public List<LetterPart> LetterParts = new List<LetterPart>();
            public Letter LetterPrefab;
        }

        [Serializable]
        public class SessionData
        {
            public int LevelId;
            public Letter LetterPrefab;
        }

        public static SessionData SessionConfiguration;
        public static RestoreData RestoreConfiguration;
        
        public static void LoadSessionData()
        {
            if (PlayerPrefs.HasKey(_sessionKey))
            {
                string json = PlayerPrefs.GetString(_sessionKey);
                SessionConfiguration = FromJson<SessionData>(json);
            }
        }

        public static void SaveSessionData()
        {
            string json = ToJson(SessionConfiguration);
            PlayerPrefs.SetString(_sessionKey, json);
            PlayerPrefs.Save();
        }

        public static void LoadRestoreData(int levelId)
        {
            var restoreKey = _restoreKey + levelId;
            if (PlayerPrefs.HasKey(restoreKey))
            {
                string json = PlayerPrefs.GetString(restoreKey);
                RestoreConfiguration = FromJson<RestoreData>(json);
            }
        }

        public static void SaveRestoreData(int levelId)
        {
            string json = ToJson(RestoreConfiguration);
            var restoreKey = _restoreKey + levelId;
            PlayerPrefs.SetString(restoreKey, json);
            PlayerPrefs.Save();
        }

        public static void ClearRestoreData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}