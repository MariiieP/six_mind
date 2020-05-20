using App;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VolumeSlider : MonoBehaviour
    {
        public enum Listener { Music, Sounds };

        [SerializeField] private Slider _slider;
        public Listener CurrentListener;
        private SettingsData _settingsData;

        private void OnEnable()
        {
            _settingsData = AppController.Instance.GetSettingsData();
            _slider.value = CurrentListener == Listener.Music ? _settingsData.MusicVolume : _settingsData.SoundVolume;

            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            AppController.Instance.SaveSettings(_settingsData);
            _slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            if (CurrentListener == Listener.Music)
            {
                AppController.Instance.SetMusicVolume(value);
                _settingsData.MusicVolume = value;
            }
            else
            {
                AppController.Instance.SetSoundVolume(value);
                _settingsData.SoundVolume = value;
            }
        }
    }
}