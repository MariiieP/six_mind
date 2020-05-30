using App;
using Data;
using Gameplay;
using UnityEngine;

namespace Utils
{
	public class Preloader : MonoBehaviour
	{
		private SettingsData _settingsData;
		[SerializeField] private LevelButton _playButton;

		private void Start()
		{
			CheckSettingsData();
			SetVolumes();
			SetDesiredLevel();
		}

		private void CheckSettingsData()
		{
			_settingsData = AppController.Instance.GetSettingsData();
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
			AppController.Instance.SaveSettings(_settingsData);
		}

		private void SetVolumes()
		{
			AppController.Instance.SetMusicVolume(_settingsData.MusicVolume);
			AppController.Instance.SetSoundVolume(_settingsData.SoundVolume);
		}

		private void SetDesiredLevel()
		{
			var levelId = AppController.Instance.GetLastPlayedLevelId();
			_playButton.LevelId = levelId;
		}
	}
}
