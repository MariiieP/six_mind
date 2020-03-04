using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
	[SerializeField] private LetterData _letterT;

	private void Start()
	{
		foreach (var obj in _letterT.Letters)
		{
			var go = Instantiate(obj.Prefab, obj.Position, Quaternion.Euler(obj.Rotation.x, obj.Rotation.y, obj.Rotation.z));

			var block = go.GetComponent<MiniBlock>();

			_blocks.Add(block);
		}

	}

	private List<MiniBlock> _blocks = new List<MiniBlock>();


	public float DistToWin = 5f;
	public GameObject WinWindow;

	private void Update()
	{
		if (CheckWin())
		{
			if (WinWindow != null)
			{
				WinWindow.SetActive(true);
			}
		}
	}

	public bool CheckWin()
	{
		var nullPosition = _blocks.Find(x => x.IsMine).StartedPosition;
		int countWinBlock = 0;
		for (int i = 0; i < _blocks.Count; i++)
		{
			var distToWin = Vector3.Distance(_blocks[i].transform.position - nullPosition, _blocks[i].StartedPosition - nullPosition);
			if (distToWin < DistToWin)
			{
				countWinBlock++;
			}
		}
		if (countWinBlock >= _blocks.Count)
		{
			return true;
		}
		return false;
	}
}
