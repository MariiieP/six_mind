using App;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
	public class HintPopup : CommonPopup
	{
		[SerializeField] private Image _hintImage;

		private void Awake()
		{
			var levelConfig = AppController.Instance.GetLevelConfig();
			var sessionManager = SessionManager.Instance;
			if (sessionManager.HintIndex > levelConfig.HintsIcons.Length - 1)
			{
				Debug.LogError("HintIndex error");
				return;
			}
			var icon = levelConfig.HintsIcons[sessionManager.HintIndex];
			_hintImage.sprite = icon;
			_hintImage.SetNativeSize();
			int nextIndex = sessionManager.HintIndex + 1;
			if (nextIndex <= levelConfig.HintsIcons.Length - 1)
			{
				sessionManager.HintIndex = nextIndex;
			}
		}
	}
}
