using UnityEngine;
using Utils;

namespace Gameplay
{
    [DisallowMultipleComponent]
    public class Letter : MonoBehaviour
    {
        [SerializeField] public LetterPart[] LetterParts;

        public void TrackCorrectData()
        {
            foreach (var part in LetterParts)
            {
                var pos = part.transform.position;
                var rot = part.transform.rotation;
                part.CorrectPosition = new Vector3(pos.x, pos.y, pos.z);
                part.CorrectRotation = new Vector3(rot.x, rot.y, rot.z);
            }
        }

        public void MixParts()
        {
            var colliders = new Collider2D[LetterParts.Length];
            var contactFilter = new ContactFilter2D();
            var partPosition = new Vector2(0, 0);
            int colliderCount = -1;
            var lowerLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            var upperRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var lowerRight = new Vector2(upperRight.x, lowerLeft.y);
            var upperLeft = new Vector2(lowerLeft.x, upperRight.y);

            for (int i = 0; i < LetterParts.Length; ++i)
            {
                do
                {
                    var positionX = Random.Range(upperLeft.x, upperRight.x);
                    var positionY = Random.Range(upperRight.y, lowerRight.y);
                    var part = LetterParts[i];
                    part.Body.position = new Vector2(positionX, positionY);
                    
                    colliderCount = part.Body.OverlapCollider(contactFilter, colliders);
                    ;
                } while (colliderCount != 0);



            }
        }
    }
}