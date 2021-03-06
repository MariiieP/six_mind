﻿using Managers;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class LetterPart : MonoBehaviour
	{
		[HideInInspector] public Rigidbody2D Body;
		[HideInInspector] public PolygonCollider2D PolygonColldier;
		[HideInInspector] public SpriteRenderer SpriteRenderer;

		[HideInInspector] public LetterPart Neighbour;
		[HideInInspector] public float NeighbourRotation;
		[HideInInspector] public float NeighbourDistance;

		private void Awake()
		{
			Body = GetComponent<Rigidbody2D>();
			SpriteRenderer = GetComponent<SpriteRenderer>();
			PolygonColldier = GetComponent<PolygonCollider2D>();
		}

		public bool NeighbourCorrect(float rotationDelta, float distanceDelta)
		{
			float rotationMin = NeighbourRotation - rotationDelta;
			float rotationMax = NeighbourRotation + rotationDelta;
			float distanceMax = NeighbourDistance + distanceDelta;

			float currentNeighbourRotation = Neighbour.transform.eulerAngles.z;

			if ((currentNeighbourRotation < rotationMin || currentNeighbourRotation > rotationMax) 
				&& (currentNeighbourRotation - 360f < rotationMin || currentNeighbourRotation - 360f > rotationMax) 
				&& (currentNeighbourRotation + 360f < rotationMin || currentNeighbourRotation + 360f > rotationMax))
			{
				return false;
			}

			var currentNeighbourDistance = (transform.position - Neighbour.transform.position).sqrMagnitude;

			if (currentNeighbourDistance > distanceMax)
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
			return Body.OverlapCollider(contactFilter, colliders);
		}

		public Collider2D[] GetOverlapCollider()
		{
			var contactFilter = new ContactFilter2D
			{
				useTriggers = false
			};
			var colliders = new Collider2D[SessionManager.Instance.LetterInstance.LetterParts.Length + 5];
			Body.OverlapCollider(contactFilter, colliders);
			return colliders;
		}

		public void SetSpriteAlpha(float alpha)
		{
			var color = SpriteRenderer.color;
			color.a = alpha;
			SpriteRenderer.color = color;
		}
	}
}