using App;
using Data;
using Gameplay;
using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Managers
{
	public class SessionManager : MonoBehaviourSingleton<SessionManager>
	{
		[SerializeField] private GameObject _boundsPrefab;
		[SerializeField] private GameObject _winPopupPrefab;

		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private float _distanceDelta;
		[SerializeField] private float _rotationDelta;
		[SerializeField] private Image _noticeImage;
		[SerializeField] private Button[] _flipButtons;
		[SerializeField] private LevelButton _nextLevelButton;
		[SerializeField] private int _moneyCount;
		public RectTransform[] BoundsCoordinates;

		public Action<Button[], bool> ButtonsFlipEvent;
		public Letter LetterInstance;

		private AppController _app => AppController.Instance;
		private FlipButtonsSwitcher _flipButtonsSwitcher;
		private SpriteFlicker _spriteFlicker;

		private void OnEnable()
		{
			InputManager.TargetDropEvent += OnTargetDropped;
			InputManager.TargetCaptureEvent += OnTargetCaptured;
			SceneLoader.SceneChangeEvent += OnSceneChangeEvent;
		}

		private void OnDisable()
		{
			InputManager.TargetDropEvent -= OnTargetDropped;
			InputManager.TargetCaptureEvent -= OnTargetCaptured;
			SceneLoader.SceneChangeEvent -= OnSceneChangeEvent;
		}

		private void Awake()
		{
			_flipButtonsSwitcher = new FlipButtonsSwitcher();
			_spriteFlicker = new SpriteFlicker();
		}

		private void Start()
		{
			SetupBounds();
			StartCoroutine(NextFrameCallback());
		}

		private IEnumerator NextFrameCallback()
		{
			yield return 0; // wait 1 frame

			var levelConfig = _app.GetLevelConfig();
			LetterInstance = Instantiate(levelConfig.LetterPrefab, _spawnPoint.position, Quaternion.identity);
			TrackCorrectData();

			_noticeImage.sprite = levelConfig.NoticeIcon;

			var restoreData = _app.GetRestoreData();
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

		private void OnTargetCaptured(LetterPart obj)
		{
			ButtonsFlipEvent.Invoke(_flipButtons, true);
		}

		private void OnTargetDropped(LetterPart obj)
		{
			ButtonsFlipEvent?.Invoke(_flipButtons, false);

			var result = CheckWin();
			if (result)
			{
				var wasAdded = _app.ProgressController.AddCompletedLevel(_app.CurrentLevelId);
				if (wasAdded) 
				{
					var firstLockedLevel = _app.ProgressController.GetFirstLockedLevelId();
					_app.ProgressController.AddUnfulfilledLevel(firstLockedLevel);
				}
				_app.ProgressController.AddMoney(_moneyCount);
				var winPopup = _app.OpenPopup(_winPopupPrefab).GetComponent<Popup>();
				winPopup.NextLevelButton.LevelId = _app.CurrentLevelId + 1;
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
			var levelConfig = _app.GetLevelConfig();

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

			_app.SaveLastSession(restoreData);
			_app.SaveLastPlayedLevelId(levelConfig.LevelId);
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
