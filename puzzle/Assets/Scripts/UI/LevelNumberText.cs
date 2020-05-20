using App;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelNumberText : MonoBehaviour
    {
        private void Awake()
        {
            var textComponent = GetComponent<Text>();
            textComponent.text = (AppController.Instance.CurrentLevelId + 1).ToString();
        }
    }

}