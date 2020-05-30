using App;
using Data;
using System.Collections.Generic;

namespace Gameplay
{
	public class LevelProgressController
	{
		private AppController _app => AppController.Instance;
		private ProgressData _progressData = AppController.Instance.GetProgressData();

		public void CreateProgressData()
		{
			var progressData = new ProgressData
			{
				UnfulfilledLevelIds = new List<int>(),
				CompletedLevelIds = new List<int>()
			};
			int firstLevelId = 0;
			progressData.UnfulfilledLevelIds.Add(firstLevelId);
			_progressData = progressData;
			_app.SaveProgressData(progressData);
		}

		public List<int> GetUnfulfilledLevels()
		{
			return _progressData.UnfulfilledLevelIds;
		}

		public List<int> GetCompletedLevels()
		{
			return _progressData.CompletedLevelIds;
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
			if (!unfulfilledLevels.Contains(levelId))
			{
				unfulfilledLevels.Add(levelId);
				_app.SaveProgressData(_progressData);
			}
		}

		public void AddCompletedLevel(int levelId)
		{
			var completedLevels = GetCompletedLevels();
			if (!completedLevels.Contains(levelId))
			{
				completedLevels.Add(levelId);
				var unfulfilledLevels = GetUnfulfilledLevels();
				unfulfilledLevels.Remove(levelId);
				_app.SaveProgressData(_progressData);
			}
		}

		public bool IsLevelLocked(int levelId)
		{
			var unfulfilledLevels = GetUnfulfilledLevels();
			var completedLevels = GetCompletedLevels();
			return !completedLevels.Contains(levelId) && !unfulfilledLevels.Contains(levelId);
		}
	}
}
