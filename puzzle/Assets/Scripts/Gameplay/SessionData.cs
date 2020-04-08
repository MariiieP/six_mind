using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Gameplay
{
	public class SessionData : MonoBehaviour
	{
		public int LevelId;
		public Letter Prefab;

		public void EntryGame()
		{
			PlayerData.SessionConfiguration.LetterPrefab = Prefab;
			PlayerData.SessionConfiguration.LevelId = LevelId;
			PlayerData.SaveSessionData();
			SceneManager.LoadScene("Game");
		}
	}
}
