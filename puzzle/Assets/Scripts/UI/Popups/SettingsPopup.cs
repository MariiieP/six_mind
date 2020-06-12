using App;
using Data;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Popups
{
	public class SettingsPopup : CommonPopup
	{
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        private PersistentData _settingsData;

        private void OnEnable()
        {
            _settingsData = AppController.Instance.GetPersistentData();

            _soundSlider.value = _settingsData.SoundVolume;
            _musicSlider.value = _settingsData.MusicVolume;

            _soundSlider.onValueChanged.AddListener(OnSoundValueChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        }

        private void OnDisable()
        {
            AppController.Instance.SavePersistentData(_settingsData);

            _soundSlider.onValueChanged.RemoveListener(OnSoundValueChanged);
            _musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
        }

        private void OnMusicValueChanged(float value)
        {
            AppController.Instance.SetMusicVolume(value);
            _settingsData.MusicVolume = value;
        }

        private void OnSoundValueChanged(float value)
        {
            AppController.Instance.SetSoundVolume(value);
            _settingsData.SoundVolume = value;
        }
    }
}
