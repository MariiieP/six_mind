using UnityEngine;
using Managers;

namespace Gameplay
{
    [DisallowMultipleComponent]
    public class Letter : MonoBehaviour
    {
        public LetterPart[] LetterParts;

        private void Awake()
        {
            GetLetterParts();
        }

        private void GetLetterParts()
        {
            var letterPartsCount = transform.childCount;
            LetterParts = new LetterPart[letterPartsCount];
            for (int i = 0; i < letterPartsCount; ++i)
            {
                LetterParts[i] = transform.GetChild(i).GetComponent<LetterPart>();
            }
        }

        public void MixParts()
        {
            var colliders = new Collider2D[LetterParts.Length + 5];
            var contactFilter = new ContactFilter2D();
            var boundsCoordinates = SessionManager.Instance.BoundsCoordinates;
            var upperLeft = boundsCoordinates[0].transform.position;
            var upperRight = boundsCoordinates[1].transform.position;
            var lowerRight = boundsCoordinates[3].transform.position;

            for (int i = 0, colliderCount; i < LetterParts.Length; ++i)
            {
                do
                {
                    var positionX = Random.Range(upperLeft.x, upperRight.x);
                    var positionY = Random.Range(upperRight.y, lowerRight.y);
                    var part = LetterParts[i];
                    part.Body.position = new Vector2(positionX, positionY);
                    part.transform.rotation = Quaternion.Euler(180f * Random.Range(0, 2), 
                                                               180f * Random.Range(0, 2), 
                                                               Random.Range(0, 360f));
                    colliderCount = part.Body.OverlapCollider(contactFilter, colliders);
                } while (colliderCount != 0);
            }
        }
    }
}