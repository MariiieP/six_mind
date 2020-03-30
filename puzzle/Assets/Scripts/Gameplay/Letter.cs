using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    public class Letter : MonoBehaviour
    {
        public LetterPart[] letterParts;

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
            var colliders = new Collider2D[letterParts.Length];
            var isFoundedPlaceColliders = new Collider2D[letterParts.Length + 5];
            var contactFilter = new ContactFilter2D();
            var partPosition = new Vector2(0, 0);
            int colliderCount = -1;
            var lowerLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            var upperRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var lowerRight = new Vector2(upperRight.x, lowerLeft.y);
            var upperLeft = new Vector2(lowerLeft.x, upperRight.y);

            for (int i = 0; i < letterParts.Length; ++i)
            {
                do
                {
                    var positionX = Random.Range(upperLeft.x, upperRight.x);
                    var positionY = Random.Range(upperRight.y, lowerRight.y);
                    var part = letterParts[i];
                    part.body.position = new Vector2(positionX, positionY);
                    part.body.rotation = Random.Range(0, 360);

                    colliderCount = part.body.OverlapCollider(contactFilter, colliders);
                    ;
                } while (colliderCount != 0);



            }
        }
    }
}