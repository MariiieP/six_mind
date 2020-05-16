using Gameplay;
using UnityEngine;
using Utils;

namespace Managers
{
	public class SessionManager : MonoBehaviourSingleton<SessionManager>
	{
		[SerializeField] private GameObject _boundsPrefab;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private float _distanceDelta;
		[SerializeField] private float _rotationDelta;
		public RectTransform[] BoundsCoordinates;

		private Letter _letterInstance;

		private void Start()
		{
			PlayerData.LoadSessionData();
			var sessionData = PlayerData.SessionConfiguration;

			PlayerData.LoadRestoreData(sessionData.LevelId);
			var restoreData = PlayerData.RestoreConfiguration;

			if (restoreData == null)
			{
				StartCleanSession();
				Debug.Log("Clean session loaded");
			}
			else
			{
				StartRestoredSession();
				Debug.Log("Restored session loaded");
			}
		}

		private void OnEnable()
		{
			InputManager.TargetDropped += OnTargetDropped;
		}

		private void OnDisable()
		{
			InputManager.TargetDropped -= OnTargetDropped;
		}

		private void OnTargetDropped()
		{
			CheckWin();
		}

		private bool CheckWin()
		{
			foreach (var letterPart in _letterInstance.letterParts)
			{
				if (!letterPart.NeighbourCorrect(_rotationDelta, _distanceDelta))
				{
					return false;
				}
			}

			return true;
		}

		public void StartCleanSession()
		{
			var sessionData = PlayerData.SessionConfiguration;

			SetupBounds();

			_letterInstance = Instantiate(sessionData.LetterPrefab, _spawnPoint.position, Quaternion.identity);
			TrackCorrectData();
			_letterInstance.MixParts();
		}

		private void StartRestoredSession()
		{
			var restoreData = PlayerData.RestoreConfiguration;

			SetupBounds();
			_letterInstance = Instantiate(restoreData.LetterPrefab, _spawnPoint.position, Quaternion.identity);
			TrackCorrectData();

			for (int i = 0; i < _letterInstance.letterParts.Length; ++i)
			{
				var part = _letterInstance.letterParts[i];
				part.transform.localPosition = restoreData.LetterParts[i].Position;
				part.transform.eulerAngles = restoreData.LetterParts[i].Rotation;
			}

		}

		private void OnApplicationQuit()
		{
			PlayerData.RestoreConfiguration = new PlayerData.RestoreData();
			var restoreData = PlayerData.RestoreConfiguration;
			var sessionData = PlayerData.SessionConfiguration;

			restoreData.LetterPrefab = sessionData.LetterPrefab;
			foreach (var letterPart in _letterInstance.letterParts)
			{
				var letterData = new PlayerData.RestoreData.LetterPart();
				letterData.Position = letterPart.transform.localPosition;
				letterData.Rotation = letterPart.transform.eulerAngles;
				restoreData.LetterParts.Add(letterData);
			}

			PlayerData.SaveRestoreData(sessionData.LevelId);
		}

		private void TrackCorrectData()
		{
			for (int currentIdx = 0, neighbourIdx = 1; currentIdx < _letterInstance.letterParts.Length; ++currentIdx, ++neighbourIdx)
			{
				if (neighbourIdx == _letterInstance.letterParts.Length - 1)
				{
					neighbourIdx = 0;
				}

				var neighbour = _letterInstance.letterParts[neighbourIdx];
				var neighbourRotation = neighbour.transform.eulerAngles.z;
				var current = _letterInstance.letterParts[currentIdx];
				var dist = current.transform.position - neighbour.transform.position;
				var neighbourDistance = Mathf.Sqrt(dist.x * dist.x + dist.y * dist.y);

				current.Neighbour = neighbour;
				current.NeighbourRotation = neighbourRotation;
				current.NeighbourDistance = neighbourDistance;
			}
		}

		private void SetupBounds()
		{
			var bounds = Instantiate(_boundsPrefab);
			for (int i = 0; i < bounds.transform.childCount; ++i)
			{
				var child = bounds.transform.GetChild(i);
				child.position = BoundsCoordinates[i].transform.position;
			}
		}
	}
}
