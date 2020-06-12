using Gameplay;
using Managers;

namespace UI.Popups.Tutorial
{
	public class LetterPartSelectionPopup : TutorialPopup
	{
		private void OnEnable()
		{
			InputManager.TargetCaptureEvent += OnTargetCaptured;
		}

		private void OnDisable()
		{
			InputManager.TargetCaptureEvent -= OnTargetCaptured;
		}

		private void OnTargetCaptured(LetterPart obj)
		{
			ClosePopup();
		}
	}
}
