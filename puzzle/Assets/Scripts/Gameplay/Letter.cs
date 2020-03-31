using UnityEngine;

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
            var lowerLeft = (Vector2)Camera.main.ScreenToWorldPoint(Vector3.zero);
            var upperRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
            var lowerRight = new Vector2(upperRight.x, lowerLeft.y);
            var upperLeft = new Vector2(lowerLeft.x, upperRight.y);
            for (int i = 0, colliderCount; i < letterParts.Length; ++i)
            {
                do
                {
                    var positionX = Random.Range(upperLeft.x, upperRight.x);
                    var positionY = Random.Range(upperRight.y, lowerRight.y);
                    var part = letterParts[i];
                    part.body.position = new Vector2(positionX, positionY);
                    part.body.rotation = Random.Range(0, 360);
                    colliderCount = part.body.OverlapCollider(contactFilter, colliders);
                } while (colliderCount != 0);
            }
        }
    }
}