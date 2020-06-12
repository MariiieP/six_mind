using Managers;

namespace UI.Popups.Tutorial
{
	public class RotatePartPopup : TutorialPopup
	{
		private void OnEnable()
		{
			TargetRotator.TargetDropEvent += OnTargetDropped;
		}

		private void OnDisable()
		{
			TargetRotator.TargetDropEvent -= OnTargetDropped;
		}

		private void OnTargetDropped()
		{
			ClosePopup();
		}
	}
}
