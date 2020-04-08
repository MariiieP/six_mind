using Gameplay;
using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.RestService;
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

		private PlayerData.RestoreData.Level _currentLevel;
		private Letter _letterInstance;

		private void Start()
		{
			PlayerData.LoadSessionData();
			PlayerData.LoadRestoreData();

			var restoreData = PlayerData.RestoreConfiguration;
			var sessionData = PlayerData.SessionConfiguration;

			_currentLevel = null;
			
			foreach (var level in restoreData.Levels)
			{
				if (level.LevelId == sessionData.LevelId)
				{
					_currentLevel = level;
				}
			}
			
			 if (_currentLevel == null)
			 {
			     _currentLevel = new PlayerData.RestoreData.Level();
			     restoreData.Levels.Add(_currentLevel);
			     StartCleanSession();
			 }
			 else
			 {
				 StartRestoredSession();
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

		private void StartCleanSession()
		{
			SetupBounds();

			_letterInstance = Instantiate(_letterPrefab, _spawnPoint.position, Quaternion.identity);
			TrackCorrectData();
			_letterInstance.MixParts();
		}

		private void StartRestoredSession()
		{
			SetupBounds();
			_letterInstance = Instantiate(_currentLevel.LetterPrefab, _spawnPoint.position, Quaternion.identity);
			TrackCorrectData();

			for (int i = 0; i < _letterInstance.letterParts.Length; ++i)
			{
				var part = _letterInstance.letterParts[i];
				part.transform.localPosition = _currentLevel.LetterParts[i].Position;
				part.transform.eulerAngles = _currentLevel.LetterParts[i].Rotation;
			}

		}

		private void OnApplicationQuit()
		{
			var restoreData = PlayerData.RestoreConfiguration;
			var sessionData = PlayerData.SessionConfiguration;

			_currentLevel.LetterPrefab = _letterPrefab;
			_currentLevel.LevelId = sessionData.LevelId;
			_currentLevel.LetterParts.Clear();
			foreach (var letterPart in _letterInstance.letterParts)
            {
                var letterData = new PlayerData.RestoreData.Level.LetterPart();
                letterData.Position = letterPart.transform.localPosition;
                letterData.Rotation = letterPart.transform.eulerAngles;
                _currentLevel.LetterParts.Add(letterData);
            }
			
			PlayerData.SaveRestoreData();
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
