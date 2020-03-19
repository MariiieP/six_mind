using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace Gameplay
{
    [DisallowMultipleComponent]
    public class Letter : MonoBehaviour
    {
        [FormerlySerializedAs("LetterParts")] public LetterPart[] letterParts; 

        public void TrackCorrectData()
        {
            foreach (var part in letterParts)
            {
                var pos = part.transform.position;
                var rot = part.transform.rotation;
                part.correctPosition = new Vector3(pos.x, pos.y, pos.z);
                part.correctRotation = new Vector3(rot.x, rot.y, rot.z);
            }
        }

        public void MixParts()
        {
            var isFoundedPlaceColliders = new Collider2D[letterParts.Length+4];
            var contactFilter = new ContactFilter2D();
            var colliderCount = -1;
            
            var lowerLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            var upperRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var lowerRight = new Vector2(upperRight.x, lowerLeft.y);
            var upperLeft = new Vector2(lowerLeft.x, upperRight.y);

            var startTime = DateTime.Now.Ticks;
            var estimatedTime = 0.0;
            foreach (var itemPart in letterParts)
            {
                while (colliderCount != 0) //&& estimatedTime < 30000
                {
                    var randomPositionX = Random.Range(upperLeft.x, upperRight.x);
                    var randomPositionY = Random.Range(upperRight.y, lowerRight.y);
                    itemPart.body.position = new Vector2(randomPositionX, randomPositionY);
                    itemPart.body.rotation = Random.Range(0, 360);

                    colliderCount = itemPart.body.OverlapCollider(contactFilter,isFoundedPlaceColliders);
                    estimatedTime = DateTime.Now.Ticks - startTime;
                }
                
                
                estimatedTime = 0.0;
                colliderCount = -1;
            }
        }
    }
}