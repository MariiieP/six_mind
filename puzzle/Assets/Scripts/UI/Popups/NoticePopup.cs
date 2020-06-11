using App;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
	public class NoticePopup : CommonPopup
	{
		[SerializeField] private Image _noticeImage;

		private void Awake()
		{
			var levelConfig = AppController.Instance.GetLevelConfig();
			_noticeImage.sprite = levelConfig.NoticeIcon;
			_noticeImage.SetNativeSize();
		}
	}
}
