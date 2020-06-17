using Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[Serializable]
	public class WinningCombinationData
	{
		[Serializable]
		public class LetterInfo
		{
			public string CurrentPartName;
			public string NeighborName;
			public float NeighborDistance;
			public float NeighborRotation;
		}
		[SerializeField] public List<LetterInfo> PartsInfo = new List<LetterInfo>();
	}
}
