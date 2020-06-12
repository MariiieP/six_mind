using Gameplay;
using Managers;

namespace UI.Popups.Tutorial
{
	public class MovePartPopup : TutorialPopup
	{
		private void OnEnable()
		{
			InputManager.TargetDropEvent += OnTargetDropped;
		}

		private void OnDisable()
		{
			InputManager.TargetDropEvent -= OnTargetDropped;
		}

		private void OnTargetDropped(LetterPart obj)
		{
			ClosePopup();
		}
	}
}
