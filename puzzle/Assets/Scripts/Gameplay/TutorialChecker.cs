using App;
using Managers;
using System.Collections;
using UI.Popups.Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class TutorialChecker : MonoBehaviour
	{
		[SerializeField] private GameObject _greetingsPopup;
		[SerializeField] private GameObject _rulesPopup;
		[SerializeField] private GameObject _partSelectionPopup;
		[SerializeField] private GameObject _movePartPopup;
		[SerializeField] private GameObject _rotatePartPopup;
		[SerializeField] private GameObject _flipPartPopup;
		[SerializeField] private GameObject _tutorialCompletePopup;

		private bool _trigger = false;

		private AppController _app => AppController.Instance;

		private void Start()
		{
			var persistentData = _app.GetPersistentData();
			if (persistentData.Tutorial)
			{
				StartCoroutine(RunTutorial());
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private IEnumerator RunTutorial()
		{
			InputManager.InputAccess = false;
			yield return ShowPopup(_greetingsPopup);
			yield return ShowPopup(_rulesPopup);

			InputManager.InputAccess = true;
			yield return ShowPopup(_partSelectionPopup);
			yield return ShowPopup(_movePartPopup);
			yield return ShowPopup(_rotatePartPopup);
			yield return ShowPopup(_flipPartPopup);
			yield return ShowPopup(_tutorialCompletePopup);

			CompleteTutorial();
			yield return null;
		}

		private IEnumerator ShowPopup(GameObject popupPrefab)
		{
			var popup = _app.InitPopup(popupPrefab).GetComponent<TutorialPopup>();
			popup.PopupCloseEvent += OnPopupClosed;
			yield return new WaitUntil(() => _trigger);
			popup.PopupCloseEvent -= OnPopupClosed;
			_trigger = false;
			yield return null;
		}

		private void CompleteTutorial()
		{
			var data = _app.GetPersistentData();
			data.Tutorial = false;
			_app.SavePersistentData(data);
			Destroy(gameObject);
		}

		private void OnPopupClosed()
		{
			_trigger = true;
		}
	}
}
