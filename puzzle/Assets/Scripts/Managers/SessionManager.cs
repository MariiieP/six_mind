using App;
using Data;
using Gameplay;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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

		public Letter LetterInstance;

		private void Start()
		{
			SetupBounds();
			StartCoroutine(NextFrameCallback());
		}

		private IEnumerator NextFrameCallback()
		{
			yield return 0; // wait 1 frame

			var levelConfig = AppController.Instance.GetLevelConfig();
			LetterInstance = Instantiate(levelConfig.LetterPrefab, _spawnPoint.position, Quaternion.identity);
			TrackCorrectData();

			_noticeImage.sprite = levelConfig.NoticeIcon;

			var restoreData = AppController.Instance.GetRestoreData();
			if (restoreData == null) // means that first time
			{
				LetterInstance.MixParts();
			}
			else
			{
				for (int i = 0; i < LetterInstance.LetterParts.Length; ++i)
				{
					var part = LetterInstance.LetterParts[i];
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
			var result = CheckWin();
			if (result)
			{
				Debug.Log("Победа!");
			}
		}

		private bool CheckWin()
		{
			foreach (var letterPart in LetterInstance.LetterParts)
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
			foreach (var letterPart in LetterInstance.LetterParts)
			{
				var letterData = new RestoreData.LetterPart
				{
					Position = letterPart.transform.localPosition,
					Rotation = letterPart.transform.eulerAngles
				};
				restoreData.LetterParts.Add(letterData);
			}

			AppController.Instance.SaveLastSession(restoreData);
			AppController.Instance.SaveLastPlayedLevelId(levelConfig.LevelId);
		}

		private void TrackCorrectData()
		{
			for (int currentIdx = 0, neighbourIdx = 1; currentIdx < LetterInstance.LetterParts.Length; ++currentIdx, ++neighbourIdx)
			{
				if (neighbourIdx == LetterInstance.LetterParts.Length - 1)
				{
					neighbourIdx = 0;
				}

				var neighbour = LetterInstance.LetterParts[neighbourIdx];
				var neighbourRotation = neighbour.transform.eulerAngles.z;
				var current = LetterInstance.LetterParts[currentIdx];
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
