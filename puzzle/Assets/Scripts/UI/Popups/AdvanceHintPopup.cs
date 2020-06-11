using App;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
	public class AdvanceHintPopup : CommonPopup
	{
		[SerializeField] private Text _priceText;
		[SerializeField] private int _cost;
		[SerializeField] private GameObject _hintPopupPrefab;

		private void Awake()
		{
			_priceText.text = _cost.ToString();
		}

		public void TryShowHint()
		{
			var app = AppController.Instance;
			var progress = app.ProgressController;
			var money = progress.GetMoney();
			if (money < _cost)
			{
				return;
			}
			progress.ReduceMoney(_cost);
			app.OpenPopup(_hintPopupPrefab);
			ClosePopup();
		}
	}
}
