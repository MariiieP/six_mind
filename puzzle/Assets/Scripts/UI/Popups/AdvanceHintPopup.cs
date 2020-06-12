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
		[SerializeField] private GameObject _notEnoughPopupPrefab;

		private void Awake()
		{
			_priceText.text = _cost.ToString();
		}

		public void TryShowHint()
		{
			var app = AppController.Instance;
			var progress = app.ProgressController;
			var money = progress.GetMoney();
			GameObject prefab;
			if (money < _cost)
			{
				prefab = _notEnoughPopupPrefab;
			}
			else
			{
				progress.ReduceMoney(_cost);
				prefab = _hintPopupPrefab;
			}
			app.OpenPopup(prefab);
			ClosePopup();
		}
	}
}
