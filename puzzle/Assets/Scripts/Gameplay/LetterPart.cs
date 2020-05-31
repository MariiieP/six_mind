using Managers;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class LetterPart : MonoBehaviour
	{
		[HideInInspector] public Rigidbody2D Body;
		[HideInInspector] public SpriteRenderer SpriteRenderer;

		[HideInInspector] public LetterPart Neighbour;
		[HideInInspector] public float NeighbourRotation;
		[HideInInspector] public float NeighbourDistance;

		private void Awake()
		{
			Body = GetComponent<Rigidbody2D>();
			SpriteRenderer = GetComponent<SpriteRenderer>();
		}

		public bool NeighbourCorrect(float rotationDelta, float distanceDelta)
		{
			float rotationMin = NeighbourRotation - rotationDelta;
			float rotationMax = NeighbourRotation + rotationDelta;

			float distanceMin = NeighbourDistance - distanceDelta;
			float distanceMax = NeighbourDistance + distanceDelta;

			float currentNeighbourRotation = Neighbour.transform.eulerAngles.z;

			if (currentNeighbourRotation < rotationMin || currentNeighbourRotation > rotationMax)
			{
				return false;
			}

			var dist = transform.position - Neighbour.transform.position;
			var currentNeighbourDistance = Mathf.Sqrt(dist.x * dist.x + dist.y * dist.y);

			if (currentNeighbourDistance < distanceMin || currentNeighbourDistance > distanceMax)
			{
				return false;
			}      

			return true;
		}

		public int GetCollisionsCount()
		{
			var contactFilter = new ContactFilter2D
			{
				useTriggers = false
			};
			var colliders = new Collider2D[SessionManager.Instance.LetterInstance.LetterParts.Length + 5];
			int collisionsCount = Body.OverlapCollider(contactFilter, colliders);
			return collisionsCount;
		}

		public void SetSpriteAlpha(float alpha)
		{
			var color = SpriteRenderer.color;
			color.a = alpha;
			SpriteRenderer.color = color;
		}
	}
}