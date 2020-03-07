using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class LetterPart : MonoBehaviour
	{
		[HideInInspector] public Vector3 CorrectPosition;
		[HideInInspector] public Vector3 CorrectRotation;

		public Rigidbody2D Body;

		private void Awake()
		{
			Body = GetComponent<Rigidbody2D>();
		}
	}
}