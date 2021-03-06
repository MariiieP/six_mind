﻿using App;
using Data;
using Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using static Data.WinningCombinationData;

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
		public int HintIndex { get; set; }

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
				HintIndex = restoreData.HintIndex;
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
					_app.ProgressController.AddMoney(_moneyCount);
				}
				var winPopup = _app.InitPopup(_winPopupPrefab).GetComponent<WinPopup>();
				winPopup.NextLevelButton.LevelId = _app.CurrentLevelId + 1;
			}
		}

		private bool CheckData(List<LetterInfo> partsInfo)
		{
			var parts = LetterInstance.LetterParts;

			for (int i = 0; i < partsInfo.Count; ++i)
			{
				var block = partsInfo[i];

				var currentLetterPart = Array.Find(parts, match => match.name == block.CurrentPartName);
				currentLetterPart.Neighbour = Array.Find(parts, match => match.name == block.NeighborName);
				currentLetterPart.NeighbourRotation = block.NeighborRotation;
				currentLetterPart.NeighbourDistance = block.NeighborDistance;

				if (!currentLetterPart.NeighbourCorrect(_rotationDelta, _distanceDelta))
				{
					return false;
				}
			}

			return true;
		}

		private bool CheckWin()
		{
			var deserializedData = AppController.Instance.GetCurrentCombinations();
			var results = new List<bool>();
			foreach (var data in deserializedData)
			{
				var partsInfo = data.PartsInfo;
				results.Add(CheckData(partsInfo));
			}
			bool result = results.Find(m => m == true);
			return result;
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
			restoreData.HintIndex = HintIndex;
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
			var correctData = new WinningCombinationData();
			var parts = LetterInstance.LetterParts;
			for (int currentIdx = 0, neighbourIdx = 1; currentIdx < LetterInstance.LetterParts.Length; ++currentIdx, ++neighbourIdx)
			{
				if (neighbourIdx == parts.Length)
				{
					neighbourIdx = 0;
				}

				var current = parts[currentIdx];
				var neighbour = parts[neighbourIdx];
				var neighbourRotation = neighbour.transform.eulerAngles.z;
				var neighbourDistance = (current.transform.position - neighbour.transform.position).sqrMagnitude;

				current.Neighbour = neighbour;
				current.NeighbourRotation = neighbourRotation;
				current.NeighbourDistance = neighbourDistance;

				var data = new LetterInfo
				{
					CurrentPartName = current.name,
					NeighborRotation = neighbour.transform.eulerAngles.z,
					NeighborName = neighbour.name,
					NeighborDistance = neighbourDistance
				};
				correctData.PartsInfo.Add(data);
			}

			Debug.Log(JsonUtility.ToJson(correctData));
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
