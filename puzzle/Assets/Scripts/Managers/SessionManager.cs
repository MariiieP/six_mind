using Gameplay;
using System;
using UnityEngine;
using Utils;

namespace Managers
{
	public class SessionManager : MonoBehaviourSingleton<SessionManager>
	{
		[SerializeField] private Letter _letterPrefab;
		[SerializeField] private GameObject _boundsPrefab;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private float _distanceDelta;
		[SerializeField] private float _rotationDelta;

		private Letter _letterInstance;

		private void Start()
		{
			StartSession();
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
			Debug.Log(CheckWin());
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

		private void StartSession()
		{
			SetupBounds();

			_letterInstance = Instantiate(_letterPrefab, _spawnPoint.position, Quaternion.identity);
			// _letterInstance.gameObject.SetActive(true);
			TrackCorrectData();
			_letterInstance.MixParts();
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

				Debug.Log($"Current: {current.name} -> Neighbor: {neighbour.name}, rotation: {neighbourRotation}, distance: {neighbourDistance}");
			}
		}

		private void SetupBounds()
		{
			Vector2[] boundsPositions = new Vector2[4];
			boundsPositions[0] = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)); // lowerLeft
			boundsPositions[1] = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); // upperRight
			boundsPositions[2] = new Vector2(boundsPositions[1].x, boundsPositions[0].y); // lowerRight
			boundsPositions[3] = new Vector2(boundsPositions[0].x, boundsPositions[1].y); // upperLeft

			var bounds = Instantiate(_boundsPrefab);
			for (int i = 0; i < bounds.transform.childCount; ++i)
			{
				var child = bounds.transform.GetChild(i);
				child.position = boundsPositions[i];
			}
		}
	}
}
