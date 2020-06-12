using System;

namespace UI.Popups.Tutorial
{
	public class TutorialPopup : CommonPopup
	{
		public Action PopupCloseEvent;

		public override void ClosePopup()
		{
			PopupCloseEvent?.Invoke();
			base.ClosePopup();
		}
	}
}
