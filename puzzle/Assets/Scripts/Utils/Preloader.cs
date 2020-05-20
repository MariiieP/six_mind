using App;
using Data;
using UnityEngine;

namespace Utils
{
	public class Preloader : MonoBehaviour
	{
		private SettingsData _settingsData;

		private void Start()
		{
			CheckSettingsData();
			SetVolumes();
		}

		private void CheckSettingsData()
		{
			_settingsData = AppController.Instance.GetSettingsData();
			if (_settingsData == null)
			{
				_settingsData = new SettingsData
				{
					MusicVolume = 0.5f,
					SoundVolume = 0.5f,
				};
				AppController.Instance.SaveSettings(_settingsData);
			}
		}

		private void SetVolumes()
		{
			AppController.Instance.SetMusicVolume(_settingsData.MusicVolume);
			AppController.Instance.SetSoundVolume(_settingsData.SoundVolume);
		}
	}
}
