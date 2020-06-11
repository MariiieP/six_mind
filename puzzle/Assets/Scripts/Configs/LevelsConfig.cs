using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay;

namespace Configs
{
	[Serializable, CreateAssetMenu(menuName = "Levels Config")]
	public class LevelsConfig : ScriptableObject
	{
		[Serializable]
		public enum Complexity { Easy, Normal, Hard };

		[Serializable]
		public class DataLevelConfig
		{
			public int LevelId;
			public Letter LetterPrefab;
			public Sprite NoticeIcon;
			public Complexity Complexity;
			public Sprite[] HintsIcons;
		}

		public List<DataLevelConfig> Levels;
	}
}
