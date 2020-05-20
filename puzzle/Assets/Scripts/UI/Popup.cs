using App;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class Popup : MonoBehaviour
	{
		[SerializeField] private GameObject _popup;
		[SerializeField] private Image _noticeImage;

		private void Awake()
		{
			if (_noticeImage != null)
			{
				var levelConfig = AppController.Instance.GetLevelConfig();
				_noticeImage.sprite = levelConfig.NoticeIcon;
				_noticeImage.SetNativeSize();
			}
		}

		public void OpenPopup()
		{
			_popup.SetActive(true);
		}

		public void ClosePopup()
		{
			_popup.SetActive(false);
		}
	}
}