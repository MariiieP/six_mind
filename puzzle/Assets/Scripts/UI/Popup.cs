using App;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class Popup : MonoBehaviour
	{
		[SerializeField] private GameObject _popup;
		[SerializeField] private Image _noticeImage;
		private List<Action> _closePopupCallbacks = new List<Action>();

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
			foreach (var callback in _closePopupCallbacks)
			{
				callback?.Invoke();
			}
			_closePopupCallbacks.Clear();
		}

		public void AddCloseCallback(Action callback)
		{
			_closePopupCallbacks.Add(callback);
		}
	}
}