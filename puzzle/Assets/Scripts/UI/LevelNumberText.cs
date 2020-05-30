using App;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelNumberText : MonoBehaviour
    {
        private enum IdType { CurrentLevel, LastPlayed };
        [SerializeField] private IdType _idType;

        private void Awake()
        {
            var textComponent = GetComponent<Text>();
            var app = AppController.Instance;
            var levelId = (_idType == IdType.CurrentLevel) ? app.CurrentLevelId : app.GetLastPlayedLevelId();
            textComponent.text = (levelId + 1).ToString();
        }
    }

}