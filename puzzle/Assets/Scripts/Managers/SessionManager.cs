using Gameplay;
using Utils;

namespace Managers
{
	public class SessionManager : MonoBehaviourSingleton<SessionManager>
	{
		public Letter Letter;
		private void Start()
		{
			Letter.gameObject.SetActive(true);
			Letter.TrackCorrectData();
			Letter.MixParts();
		}

		public void StartSession(SessionRequest request)
		{
			var letter = Instantiate(request.Prefab);
			letter.gameObject.SetActive(false);

		}
	}
}
