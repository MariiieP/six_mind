using UnityEngine;
using Managers;

namespace Gameplay
{
    [DisallowMultipleComponent]
    public class Letter : MonoBehaviour
    {
        [HideInInspector] public LetterPart[] LetterParts;

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
            var boundsCoordinates = SessionManager.Instance.BoundsCoordinates;
            var upperLeft = boundsCoordinates[0].transform.position;
            var upperRight = boundsCoordinates[1].transform.position;
            var lowerRight = boundsCoordinates[3].transform.position;

            const int maxTryCount = 1000;
            int tryCount = 0;

            for (int i = 0, colliderCount; i < LetterParts.Length; ++i)
            {
                do
                {
                    var part = LetterParts[i];

                    var positionX = Random.Range(upperLeft.x, upperRight.x);
                    var positionY = Random.Range(upperRight.y, lowerRight.y);
                    part.Body.position = new Vector2(positionX, positionY);

                    var rotationX = 180f * Random.Range(0, 2);
                    var rotationY = 180f * Random.Range(0, 2);
                    var rotationZ = Random.Range(0, 360f);
                    part.transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

                    colliderCount = part.GetCollisionsCount();

                    ++tryCount;

                    if (tryCount >= maxTryCount)
                    {
                        Debug.LogError("Number of attempts exceeded");
                        break;
                    }

                } while (colliderCount != 0);
            }
        }
    }
}