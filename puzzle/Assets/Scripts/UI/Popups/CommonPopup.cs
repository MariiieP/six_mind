using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Popups
{
	public class CommonPopup : MonoBehaviour
	{
		public virtual void ClosePopup()
		{
			Destroy(gameObject);
		}
	}
}