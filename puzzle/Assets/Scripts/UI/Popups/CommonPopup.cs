using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Popups
{
	public class CommonPopup : MonoBehaviour
	{
		protected List<Action> ClosePopupCallbacks = new List<Action>();

		public void ClosePopup()
		{
			foreach (var callback in ClosePopupCallbacks)
			{
				callback?.Invoke();
			}
			Destroy(gameObject);
		}

		public void AddCloseCallback(Action callback)
		{
			ClosePopupCallbacks.Add(callback);
		}
	}
}