using App;
using Data;
using System;
using System.Collections.Generic;
using Utils;

namespace Gameplay
{
	public class LevelProgressController
	{
		private AppController _app => AppController.Instance;
		private ProgressData _cachedProgressData = AppController.Instance.GetProgressData();

		public LevelProgressController()
		{
			SceneLoader.SceneChangeEvent += OnSceneChanged;
		}

		~LevelProgressController()
		{
			SceneLoader.SceneChangeEvent -= OnSceneChanged;
		}

		public void CreateProgressData()
		{
			var progressData = new ProgressData
			{
				UnfulfilledLevelIds = new List<int>(),
				CompletedLevelIds = new List<int>()
			};
			int firstLevelId = 0;
			progressData.UnfulfilledLevelIds.Add(firstLevelId);
			_app.SaveProgressData(progressData);
		}

		public List<int> GetUnfulfilledLevels()
		{
			return _cachedProgressData.UnfulfilledLevelIds;
		}

		public List<int> GetCompletedLevels()
		{
			return _cachedProgressData.CompletedLevelIds;
		}

		public List<int> GetLockedLevels()
		{
			var levels = _app.LevelsConfig.Levels;
			var lockedLevels = new List<int>();

			foreach (var level in levels)
			{
				var id = level.LevelId;
				if (IsLevelLocked(id))
				{
					lockedLevels.Add(id);
				}
			}

			return lockedLevels;
		}

		public void AddUnfulfilledLevel(int levelId)
		{
			var unfulfilledLevels = GetUnfulfilledLevels();
			if (IsLevelLocked(levelId))
			{
				unfulfilledLevels.Add(levelId);
				_app.SaveProgressData(_cachedProgressData);
			}
		}

		public bool AddCompletedLevel(int levelId)
		{
			bool wasAdded = false;
			var completedLevels = GetCompletedLevels();
			if (!completedLevels.Contains(levelId))
			{
				completedLevels.Add(levelId);
				var unfulfilledLevels = GetUnfulfilledLevels();
				unfulfilledLevels.Remove(levelId);
				_app.SaveProgressData(_cachedProgressData);
				wasAdded = true;
			}
			return wasAdded;
		}

		public bool IsLevelLocked(int levelId)
		{
			var unfulfilledLevels = GetUnfulfilledLevels();
			var completedLevels = GetCompletedLevels();
			return !completedLevels.Contains(levelId) && !unfulfilledLevels.Contains(levelId);
		}

		public int GetFirstLockedLevelId()
		{
			return GetLockedLevels()[0];
		}


		private void OnSceneChanged()
		{
			_cachedProgressData = AppController.Instance.GetProgressData();
		}
	}
}
