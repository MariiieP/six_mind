﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[Serializable]
	public class ProgressData
	{
		[SerializeField] public int Money;
		[SerializeField] public List<int> UnfulfilledLevelIds;
		[SerializeField] public List<int> CompletedLevelIds;
	}
}
