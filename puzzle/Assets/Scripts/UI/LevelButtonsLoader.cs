using App;
using Data;
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
		[SerializeField] private ScrollRect _buttonsScrollRect;

		private AppController _app => AppController.Instance;

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
			for (int levelId = 0; levelId < _app.LevelsConfig.Levels.Count; ++levelId)
			{
				var buttonGameObject = Instantiate(_levelButtonPrefab, _buttonsLayout);
				var levelButton = buttonGameObject.GetComponent<LevelButton>();

				levelButton.LevelId = levelId;
				if (!_app.LevelProgressController.IsLevelLocked(levelId))
				{
					levelButton.ButtonText.text = (levelId + 1).ToString(); // потому что с 0
				}

				var sprite = GetLevelButtonSprite(levelId);
				levelButton.ButtonImage.sprite = sprite;
			}
			_buttonsScrollRect.verticalNormalizedPosition = 1f;
		}


		private Sprite GetLevelButtonSprite(int levelId) // TODO: обдумать простыню
		{
			var unfulfilledLevelIds = _app.LevelProgressController.GetUnfulfilledLevels();
			var completedLevelIds = _app.LevelProgressController.GetCompletedLevels();
			var dataLevelConfig = _app.LevelsConfig.Levels[levelId];

			Sprite result = null;

			if (unfulfilledLevelIds.Contains(levelId))
			{
				if (dataLevelConfig.Complexity == Complexity.Easy)
				{
					result = _levelEasyPlayIcon;
				}
				else if (dataLevelConfig.Complexity == Complexity.Normal)
				{
					result = _levelNormalPlayIcon;
				}
				else if (dataLevelConfig.Complexity == Complexity.Hard)
				{
					result = _levelHardPlayIcon;
				}
			}
			else if (completedLevelIds.Contains(levelId))
			{
				if (dataLevelConfig.Complexity == Complexity.Easy)
				{
					result = _levelEasyDoneIcon;
				}
				else if (dataLevelConfig.Complexity == Complexity.Normal)
				{
					result = _levelNormalDoneIcon;
				}
				else if (dataLevelConfig.Complexity == Complexity.Hard)
				{
					result = _levelHardDoneIcon;
				}
			}
			else // means unavailable
			{
				if (dataLevelConfig.Complexity == Complexity.Easy)
				{
					result = _levelEasyLockedIcon;
				}
				else if (dataLevelConfig.Complexity == Complexity.Normal)
				{
					result = _levelNormalLockedIcon;
				}
				else if (dataLevelConfig.Complexity == Complexity.Hard)
				{
					result = _levelHardLockedIcon;
				}
			}

			return result;
		}
	}
}