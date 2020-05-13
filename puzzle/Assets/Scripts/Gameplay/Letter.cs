﻿using UnityEngine;
using Managers;

namespace Gameplay
{
    [DisallowMultipleComponent]
    public class Letter : MonoBehaviour
    {
        public LetterPart[] letterParts;

        public void MixParts()
        {
            var colliders = new Collider2D[letterParts.Length + 5];
            var contactFilter = new ContactFilter2D();
            var boundsCoordinates = SessionManager.Instance.BoundsCoordinates;
            var upperLeft = boundsCoordinates[0].transform.position;
            var upperRight = boundsCoordinates[1].transform.position;
            var lowerLeft = boundsCoordinates[2].transform.position;
            var lowerRight = boundsCoordinates[3].transform.position;

            for (int i = 0, colliderCount; i < letterParts.Length; ++i)
            {
                do
                {
                    var positionX = Random.Range(upperLeft.x, upperRight.x);
                    var positionY = Random.Range(upperRight.y, lowerRight.y);
                    var part = letterParts[i];
                    part.body.position = new Vector2(positionX, positionY);
                    part.transform.rotation = Quaternion.Euler(180f * Random.Range(0, 2), 
                                                               180f * Random.Range(0, 2), 
                                                               Random.Range(0, 361f));
                    //part.body.rotation = Random.Range(0, 360);
                    colliderCount = part.body.OverlapCollider(contactFilter, colliders);
                } while (colliderCount != 0);
            }
        }
    }
}