using App;
using Data;
using Gameplay;
using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
	public class Preloader : MonoBehaviour
	{
		private SettingsData _settingsData;
		[SerializeField] private LevelButton _playButton;
		private AppController _app => AppController.Instance;

		private void Start()
		{
			CheckSettingsData();
			CheckProgressData();
			SetVolumes();
			SetDesiredLevel();
		}

		#region SettingsData
		private void CheckSettingsData()
		{
			_settingsData = _app.GetSettingsData();
			if (_settingsData == null)
			{
				CreateSettingsData();
			}
		}

		private void CreateSettingsData()
		{
			_settingsData = new SettingsData
			{
				MusicVolume = 0.5f,
				SoundVolume = 0.5f,
			};
			_app.SaveSettings(_settingsData);
		}
		#endregion

		private void CheckProgressData()
		{
			var progressData = _app.GetProgressData();
			if (progressData == null)
			{
				_app.LevelProgressController.CreateProgressData();
			}
		}

		private void SetVolumes()
		{
			_app.SetMusicVolume(_settingsData.MusicVolume);
			_app.SetSoundVolume(_settingsData.SoundVolume);
		}

		private void SetDesiredLevel()
		{
			var levelId = _app.GetLastPlayedLevelId();
			_playButton.LevelId = levelId;
		}
	}
}
