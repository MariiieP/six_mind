using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
	public class SceneLoader : MonoBehaviour
	{
		public static Action SceneChangeEvent;

		public void OpenScene(string name)
		{
			SceneChangeEvent?.Invoke();
			SceneManager.LoadScene(name);
		}
	}
}