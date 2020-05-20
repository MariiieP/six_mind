using App;
using Data;
using Gameplay;
using UnityEngine;
using Utils;
using System;
using UnityEngine.UI;

namespace Managers
{
	public class SessionManager : MonoBehaviourSingleton<SessionManager>
	{
		[SerializeField] private GameObject _boundsPrefab;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private float _distanceDelta;
		[SerializeField] private float _rotationDelta;
		[SerializeField] private Image _noticeImage; 
		public RectTransform[] BoundsCoordinates;

		private Letter _letterInstance;

		private void Start()
		{
			var levelConfig = AppController.Instance.GetLevelConfig();
			SetupBounds();

			_letterInstance = Instantiate(levelConfig.LetterPrefab, _spawnPoint.position, Quaternion.identity);
			TrackCorrectData();

			_noticeImage.sprite = levelConfig.NoticeIcon;

			var restoreData = AppController.Instance.GetRestoreData();
			if (restoreData == null) // means that first time
			{
				_letterInstance.MixParts();
			}
			else
			{
				for (int i = 0; i < _letterInstance.letterParts.Length; ++i)
				{
					var part = _letterInstance.letterParts[i];
					part.transform.localPosition = restoreData.LetterParts[i].Position;
					part.transform.eulerAngles = restoreData.LetterParts[i].Rotation;
				}
			}
		}

		private void OnEnable()
		{
			InputManager.TargetDropped += OnTargetDropped;
			SceneLoader.SceneChangeEvent += OnSceneChangeEvent;
		}

		private void OnDisable()
		{
			InputManager.TargetDropped -= OnTargetDropped;
			SceneLoader.SceneChangeEvent -= OnSceneChangeEvent;
		}

		private void OnTargetDropped(LetterPart obj)
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

		private void OnApplicationQuit()
		{
			SaveSessionData();
		}

		private void OnSceneChangeEvent()
		{
			SaveSessionData();
		}

		private void SaveSessionData()
		{
			var restoreData = new RestoreData();
			var levelConfig = AppController.Instance.GetLevelConfig();

			restoreData.LetterPrefab = levelConfig.LetterPrefab;
			foreach (var letterPart in _letterInstance.letterParts)
			{
				var letterData = new RestoreData.LetterPart
				{
					Position = letterPart.transform.localPosition,
					Rotation = letterPart.transform.eulerAngles
				};
				restoreData.LetterParts.Add(letterData);
			}

			AppController.Instance.SaveLastSession(restoreData);
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
