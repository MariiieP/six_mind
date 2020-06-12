using App;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class MoneyCountText : MonoBehaviour
	{
		[SerializeField] private Text _text;
		private ProgressController _progress => AppController.Instance.ProgressController;

		private void OnEnable()
		{
			if (_progress != null)
			{
				_progress.MoneyChangeEvent += OnMoneyChanged;
			}
			OnMoneyChanged();
		}

		private void OnDisable()
		{
			if (_progress != null)
			{
				_progress.MoneyChangeEvent -= OnMoneyChanged;
			}
		}

		private void OnMoneyChanged()
		{
			_text.text = _progress.GetMoney().ToString();
		}
	}
}
