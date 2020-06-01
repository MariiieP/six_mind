﻿using Gameplay;
using Configs;
using Data;
using UnityEngine;
using Utils;

namespace App
{
	public class AppController : MonoBehaviourSingleton<AppController>
	{
		public int CurrentLevelId { get; set; }
		public LevelProgressController LevelProgressController;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
			LevelProgressController = new LevelProgressController();
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

		#endregion

		#region Data
		public LevelsConfig LevelsConfig;

		public RestoreData GetRestoreData()
		{
			return PlayerData.LoadData<RestoreData>(KeyList.RestoreKey, CurrentLevelId.ToString());
		}

		public SettingsData GetSettingsData()
		{
			return PlayerData.LoadData<SettingsData>(KeyList.SettingsKey, string.Empty);
		}

		public LevelsConfig.DataLevelConfig GetLevelConfig()
		{
			return LevelsConfig.Levels.Find(match => match.LevelId == CurrentLevelId);
		}

		public void SaveLastSession(RestoreData data)
		{
			PlayerData.SaveData(data, KeyList.RestoreKey, CurrentLevelId.ToString());
		}

		public void SaveSettings(SettingsData data)
		{
			PlayerData.SaveData(data, KeyList.SettingsKey, string.Empty);
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