using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LetterPartDocker : MonoBehaviour
    {
        [SerializeField] private float _dockDistance;
        [SerializeField] private float _dockSpeed;

        private float _rotationTime = 0.3f;
        private float _currentRotationTime = 0f;

        private void OnEnable()
        {
            InputManager.TargetDropped += DockLetterPart;
        }

        private void OnDisable()
        {
            InputManager.TargetDropped -= DockLetterPart;
        }

        private void DockLetterPart(LetterPart part)
        {
            var nearest = GetNearestLetterPart(part);
            var distance = GetDistanceBetween(nearest, part);
            if (distance <= _dockDistance)
            {
                StartCoroutine(MovePart(part, nearest));
            }
        }

        private IEnumerator MovePart(LetterPart part, LetterPart nearest)
        {
            part.Body.constraints = RigidbodyConstraints2D.None;

            Vector3 direction = Vector3.zero;
            var nearestPosition = nearest.transform.position;
            var partPosition = part.transform.position;

            if (ApproximatelyEquals(nearestPosition.y, partPosition.y))
            {
                direction += (nearestPosition.x > partPosition.x) ? Vector3.right : Vector3.left;
                part.Body.constraints |= RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                direction += (nearestPosition.y > partPosition.y) ? Vector3.up : Vector3.down;
                part.Body.constraints |= RigidbodyConstraints2D.FreezePositionX;
            }

            while (_rotationTime >= _currentRotationTime)
            {
                if (part.GetCollisionsCount() > 0)
                {
                    _currentRotationTime += Time.deltaTime;
                }

                var newPosition = direction * _dockSpeed * Time.deltaTime;
                part.Body.MovePosition(part.transform.position + newPosition);
                yield return null;
            }

            part.Body.constraints =
                    RigidbodyConstraints2D.FreezePositionX
                    | RigidbodyConstraints2D.FreezePositionY
                    | RigidbodyConstraints2D.FreezeRotation;
            _currentRotationTime = 0f;
            Debug.Log("Done!");
        }

        private LetterPart GetNearestLetterPart(LetterPart currentPart)
        {
            var letter = SessionManager.Instance.LetterInstance;
            float maxDistance = float.MaxValue;
            LetterPart desiredPart = null;

            foreach (var suspect in letter.LetterParts)
            {
                if (suspect != currentPart)
                {
                    var currentDistance = GetDistanceBetween(suspect, currentPart);
                    if (currentDistance < maxDistance)
                    {
                        maxDistance = currentDistance;
                        desiredPart = suspect;
                    }
                }
            }

            return desiredPart;
        }

        private float GetDistanceBetween(LetterPart one, LetterPart another)
        {
            var onePosition = one.transform.position;
            var anotherPosition = another.transform.position;
            var distanceVector = onePosition - anotherPosition;
            return Mathf.Sqrt(distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y);
        }

        private bool ApproximatelyEquals(float one, float another)
        {
            return Mathf.RoundToInt(Mathf.Abs(one)) == Mathf.RoundToInt(Mathf.Abs(another));
        }
    }
}