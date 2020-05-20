﻿using App;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay
{
	public class LevelButton : MonoBehaviour
	{
		public int LevelId { get; set; }
		public Text ButtonText;
		public Image ButtonImage;

		public void EntryCurrentLevel()
		{
			AppController.Instance.CurrentLevelId = LevelId;
			SceneManager.LoadScene("Game");
		}
	}
}
