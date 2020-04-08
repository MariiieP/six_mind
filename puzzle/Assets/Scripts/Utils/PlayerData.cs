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
            public class Level
            {
                [Serializable]
                public struct LetterPart 
                  {
                      public Vector3 Rotation;
                      public Vector3 Position;
                  }
    
                public List<LetterPart> LetterParts = new List<LetterPart>();
                public Letter LetterPrefab;
                public int LevelId;
            }

            public List<Level> Levels = new List<Level>();
        }

        [Serializable]
        public class SessionData
        {
            public int LevelId;
            public Letter LetterPrefab;
        }
        
        public static SessionData SessionConfiguration = new SessionData();
        public static RestoreData RestoreConfiguration = new RestoreData();
        
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

        public static void LoadRestoreData()
        {
            if (PlayerPrefs.HasKey(_restoreKey))
            {
                string json = PlayerPrefs.GetString(_restoreKey);
                RestoreConfiguration = FromJson<RestoreData>(json);
            }
        }

        public static void SaveRestoreData()
        {
            string json = ToJson(RestoreConfiguration);
            PlayerPrefs.SetString(_restoreKey, json);
            PlayerPrefs.Save();
        }
    }
}