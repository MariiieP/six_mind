using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class LetterPart : MonoBehaviour
	{
		[HideInInspector] public Vector3 correctPosition;
		[HideInInspector] public Vector3 correctRotation;
		[HideInInspector] public Rigidbody2D body;

		private void Awake()
		{
			body = GetComponent<Rigidbody2D>();
		}
	}
}