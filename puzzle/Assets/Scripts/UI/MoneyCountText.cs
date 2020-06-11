using App;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class MoneyCountText : MonoBehaviour
	{
		private void Awake()
		{
			GetComponent<Text>().text = AppController.Instance.ProgressController.GetMoney().ToString();
		}
	}
}
