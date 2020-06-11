using Managers;
using UnityEngine.UI;

namespace UI
{
	public class FlipButtonsSwitcher
	{
		public FlipButtonsSwitcher()
		{
			SessionManager.Instance.ButtonsFlipEvent += OnButtonsFlipped;
		}

		~FlipButtonsSwitcher()
		{
			SessionManager.Instance.ButtonsFlipEvent -= OnButtonsFlipped;
		}

		private void OnButtonsFlipped(Button[] buttons, bool interactable)
		{
			foreach (var button in buttons)
			{
				button.interactable = interactable;
			}
		}
	}
}
