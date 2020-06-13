using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace App
{
	public class Preloader : MonoBehaviour
	{
		private PersistentData _settingsData;
		[SerializeField] private GameObject _playButton;
		private AppController _app => AppController.Instance;

		private void Start()
		{
			CheckClearData();
			CheckSettingsData();
			CheckProgressData();
			SetVolumes();
			_app.InitProgressCotroller();
			_playButton.SetActive(true);
		}

		private void CheckClearData()
		{
			if (!PlayerPrefs.HasKey(KeyList.FirstLoadClearKey))
			{
				PlayerData.ClearAllData();
				PlayerPrefs.SetInt(KeyList.FirstLoadClearKey, 1);
			}
		}

		private void CheckSettingsData()
		{
			_settingsData = _app.GetPersistentData();
			if (_settingsData == null)
			{
				CreateSettingsData();
			}
		}

		private void CreateSettingsData()
		{
			_settingsData = new PersistentData
			{
				MusicVolume = 0.5f,
				SoundVolume = 0.5f,
				Tutorial = true,
			};
			_app.SavePersistentData(_settingsData);
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
			if (_app.ProgressController.IsLevelLocked(levelId))
			{
				levelId = 0;
			}
			_app.CurrentLevelId = levelId;
			SceneManager.LoadScene("Game");
		}
	}
}
