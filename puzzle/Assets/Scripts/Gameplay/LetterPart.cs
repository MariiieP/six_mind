using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class LetterPart : MonoBehaviour
	{
		public Vector3 CorrectPosition;
		public Vector3 CorrectRotation;

		public Rigidbody2D Body;

		private void Awake()
		{
			Body = GetComponent<Rigidbody2D>();
		}
	}
}