using App;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay
{
	public class LevelButton : MonoBehaviour
	{
		[SerializeField] private GameObject _levelInDevelopingPopup;

		public int LevelId { get; set; }
		public Text ButtonText;
		public Image ButtonImage;
		

		public void EntryCurrentLevel()
		{
			var app = AppController.Instance;
			if (LevelId+1 > 9)
			{
				app.OpenPopup(_levelInDevelopingPopup);
				return;
			}

			if (app.ProgressController.IsLevelLocked(LevelId))
			{
				return;
			}

			app.CurrentLevelId = LevelId;
			SceneManager.LoadScene("Game");
		}
	}
}
