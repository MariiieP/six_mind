using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App
{
	public class Preloader : MonoBehaviour
	{
		private SettingsData _settingsData;
		[SerializeField] private GameObject _playButton;
		private AppController _app => AppController.Instance;

		private void Start()
		{
			CheckSettingsData();
			CheckProgressData();
			SetVolumes();
			_playButton.SetActive(true);
		}

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

		private void CheckProgressData()
		{
			var progressData = _app.GetProgressData();
			if (progressData == null)
			{
				progressData = new ProgressData
				{
					Money = 0,
					UnfulfilledLevelIds = new List<int>(),
					CompletedLevelIds = new List<int>()
				};
				int firstLevelId = 0;
				progressData.UnfulfilledLevelIds.Add(firstLevelId);
				_app.SaveProgressData(progressData);
			}
		}

		private void SetVolumes()
		{
			_app.SetMusicVolume(_settingsData.MusicVolume);
			_app.SetSoundVolume(_settingsData.SoundVolume);
		}

		public void OnPlayButtonClick()
		{
			var levelId = _app.GetLastPlayedLevelId();
			if (_app.LevelProgressController.IsLevelLocked(levelId))
			{
				levelId = 0;
			}
			_app.CurrentLevelId = levelId;
			SceneManager.LoadScene("Game");
		}
	}
}
