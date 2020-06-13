using Gameplay;

namespace UI.Popups.Tutorial
{
	public class FlipPartPopup : TutorialPopup
	{
		private void OnEnable()
		{
			TargetFlipper.TargetFlipEvent += OnTargetFlipped;
		}

		private void OnDisable()
		{
			TargetFlipper.TargetFlipEvent -= OnTargetFlipped;
		}

		private void OnTargetFlipped()
		{
			ClosePopup();
		}
	}
}
