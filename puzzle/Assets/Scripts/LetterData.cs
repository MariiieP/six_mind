using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLetterData", menuName = "ScriptableObjects/LetterData")]
public class LetterData : ScriptableObject
{
	[Serializable]
	public struct Object
	{
		[SerializeField] public GameObject Prefab;
		[SerializeField] public Vector3 Position;
		[SerializeField] public Vector3 Rotation;
	}

	public Object[] Letters;
}
