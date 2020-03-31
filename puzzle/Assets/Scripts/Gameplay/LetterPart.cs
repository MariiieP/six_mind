using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class LetterPart : MonoBehaviour
	{
		[HideInInspector] public Rigidbody2D body;

		public LetterPart Neighbour;
		public float NeighbourRotation;
		public float NeighbourDistance;

		private void Awake()
		{
			body = GetComponent<Rigidbody2D>();
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

	}
}