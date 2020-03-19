using Gameplay;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Managers
{
	public class SessionManager : MonoBehaviourSingleton<SessionManager>
	{
		[FormerlySerializedAs("Letter")] public Letter letter;
		[FormerlySerializedAs("BoundsPrefab")] [SerializeField] private GameObject boundsPrefab;

		private void Start()
		{
			SetupBounds();
			letter.gameObject.SetActive(true);
			letter.TrackCorrectData();
			letter.MixParts();
		}

		public void StartSession(SessionRequest request)
		{
			var letter = Instantiate(request.Prefab);
			letter.gameObject.SetActive(false);

		}

		private void SetupBounds()
		{
			Vector2[] boundsPositions = new Vector2[4];
			boundsPositions[0] = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)); // lowerLeft
			boundsPositions[1] = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); // upperRight
			boundsPositions[2] = new Vector2(boundsPositions[1].x, boundsPositions[0].y); // lowerRight
			boundsPositions[3] = new Vector2(boundsPositions[0].x, boundsPositions[1].y); // upperLeft

			var bounds = Instantiate(boundsPrefab);
			for (var i = 0; i < bounds.transform.childCount; ++i)
			{
				var child = bounds.transform.GetChild(i);
				child.position = boundsPositions[i];
			}
		}
	}
}
