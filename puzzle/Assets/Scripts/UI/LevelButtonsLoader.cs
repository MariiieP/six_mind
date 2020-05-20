using App;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Complexity = Configs.LevelsConfig.Complexity;

namespace UI
{
	public class LevelButtonsLoader : MonoBehaviour
	{
		[SerializeField] private LevelButton _levelButtonPrefab;
		[SerializeField] private RectTransform _buttonsLayout;

		#region Icons

		[SerializeField] private Sprite _levelEasyDoneIcon;
		[SerializeField] private Sprite _levelEasyLockedIcon;
		[SerializeField] private Sprite _levelEasyPlayIcon;

		[SerializeField] private Sprite _levelHardDoneIcon;
		[SerializeField] private Sprite _levelHardLockedIcon;
		[SerializeField] private Sprite _levelHardPlayIcon;

		[SerializeField] private Sprite _levelNormalDoneIcon;
		[SerializeField] private Sprite _levelNormalLockedIcon;
		[SerializeField] private Sprite _levelNormalPlayIcon;

		#endregion

		private void Start()
		{
			var levelsList = AppController.Instance.LevelsConfig.Levels;

			for (int levelId = 0; levelId < levelsList.Count; ++levelId) 
			{
				var dataLevelConfig = levelsList[levelId];
				var buttonGameObject = Instantiate(_levelButtonPrefab, _buttonsLayout);
				var levelButton = buttonGameObject.GetComponent<LevelButton>();

				levelButton.LevelId = levelId;
				levelButton.ButtonText.text = (levelId + 1).ToString(); // потому что с 0

				switch (dataLevelConfig.Complexity) // до тех пор пока не сделаем проверку доступности уровня
				{
					case Complexity.Easy:
						levelButton.ButtonImage.sprite = _levelEasyPlayIcon;
						break;
					case Complexity.Normal:
						levelButton.ButtonImage.sprite = _levelNormalPlayIcon;
						break;
					case Complexity.Hard:
						levelButton.ButtonImage.sprite = _levelHardPlayIcon;
						break;
				}
			}
		}
	}
}