using App;
using UnityEngine;

namespace Sound
{
    public class ButtonClickPlaySound : MonoBehaviour
    {
        [SerializeField] AudioClip _sound;

        public void PlaySound()
        {
            AppController.Instance.PlaySound(_sound);
        }
    }
}