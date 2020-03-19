using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class LetterPart : MonoBehaviour
	{
		[FormerlySerializedAs("CorrectPosition")] [HideInInspector] public Vector3 correctPosition;
		[FormerlySerializedAs("CorrectRotation")] [HideInInspector] public Vector3 correctRotation;

		[FormerlySerializedAs("Body")] public Rigidbody2D body;

		private void Awake()
		{
			body = GetComponent<Rigidbody2D>();
		}
	}
}