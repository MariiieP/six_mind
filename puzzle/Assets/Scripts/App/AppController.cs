﻿using Gameplay;
using Configs;
using Data;
using UnityEngine;
using Utils;
using System;
using System.Collections.Generic;

namespace App
{
	public class AppController : MonoBehaviourSingleton<AppController>
	{
		public int CurrentLevelId { get; set; }
		public ProgressController ProgressController;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void InitProgressCotroller()
		{
			ProgressController = new ProgressController();
		}

		public void OpenPopup(GameObject popupPrefab)
		{
			Instantiate(popupPrefab);
		}

		public GameObject InitPopup(GameObject popupPrefab)
		{
			return Instantiate(popupPrefab);
		}

		#region Sounds & Music

		[SerializeField] private AudioSource _musicSource;
		[SerializeField] private AudioSource _soundSource;

		public void SetMusicVolume(float value)
		{
			_musicSource.volume = value;
		}

		public void SetSoundVolume(float value)
		{
			_soundSource.volume = value;
		}

		public void PlaySound(AudioClip sound)
		{
			_soundSource.clip = sound;
			_soundSource.Play();
		}

		#endregion

		#region Data
		public LevelsConfig LevelsConfig;

		public RestoreData GetRestoreData()
		{
			return PlayerData.LoadData<RestoreData>(KeyList.RestoreKey, CurrentLevelId.ToString());
		}

		public PersistentData GetPersistentData()
		{
			return PlayerData.LoadData<PersistentData>(KeyList.PersistentKey, string.Empty);
		}

		public List<WinningCombinationData> GetCurrentCombinations()
		{
			var levelCfg = GetLevelConfig();
			var dataJsons = levelCfg.CorrectDataJsons;
			var deserializedData = new List<WinningCombinationData>();

			foreach (var json in dataJsons)
			{
				deserializedData.Add(JsonUtility.FromJson<WinningCombinationData>(json));
			}

			return deserializedData;
		}

		public LevelsConfig.DataLevelConfig GetLevelConfig()
		{
			return LevelsConfig.Levels.Find(match => match.LevelId == CurrentLevelId);
		}

		public void SaveLastSession(RestoreData data)
		{
			PlayerData.SaveData(data, KeyList.RestoreKey, CurrentLevelId.ToString());
		}

		public void SavePersistentData(PersistentData data)
		{
			PlayerData.SaveData(data, KeyList.PersistentKey, string.Empty);
		}

		public void SaveLastPlayedLevelId(int id)
		{
			PlayerData.SaveInt(id, KeyList.LastPlayedLevelIdKey, string.Empty);
		}

		public int GetLastPlayedLevelId()
		{
			return PlayerData.LoadInt(KeyList.LastPlayedLevelIdKey, string.Empty);
		}

		public void SaveProgressData(ProgressData data)
		{
			PlayerData.SaveData(data, KeyList.ProgressKey, string.Empty);
		}

		public ProgressData GetProgressData()
		{
			return PlayerData.LoadData<ProgressData>(KeyList.ProgressKey, string.Empty);
		}

		#endregion
	}
}