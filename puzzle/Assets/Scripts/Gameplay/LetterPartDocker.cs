using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LetterPartDocker : MonoBehaviour
    {
        private class MathVector
        {
            public Vector2 A;
            public Vector2 B;
            public Vector2 Middle;
        }

        [SerializeField] private Transform[] _dots;
        [SerializeField] private float _rotationMax;
        [SerializeField] private float _distanceMax;

        [SerializeField] private float _dockSpeed;

        private void OnEnable()
        {
            InputManager.TargetDropped += OnTargetDropped;
        }

        private void OnDisable()
        {
            InputManager.TargetDropped -= OnTargetDropped;
        }

        private void OnTargetDropped(LetterPart one)
        {
            LetterPart another = GetNearestLetterPart(one);
            var distance = GetDistanceBetween(one.transform.position, another.transform.position);
            if (distance > _distanceMax)
            {
                return;
            }

            var oneVectors = GetMathVectors(one.PolygonColldier);
            var anotherVectors = GetMathVectors(another.PolygonColldier);

            var oneClosestVector = GetClosestVector(oneVectors, another.transform.position);
            var anotherClosestVector = GetClosestVector(anotherVectors, one.transform.position);

            var rotationAngle = CalcArcCosBetweenVectors(oneClosestVector, anotherClosestVector);

            if (!float.IsNaN(rotationAngle))
            {
                if (rotationAngle > 90f)
                {
                    rotationAngle = Mathf.Abs(rotationAngle - 180f);
                }

                if (rotationAngle < _rotationMax)
                {
                    one.Body.SetRotation(one.Body.rotation - rotationAngle);
                    Debug.Log("Done!");
                }
            }

            Debug.Log(rotationAngle);
            Debug.Log(another);

            _dots[0].position = oneClosestVector.A;
            _dots[1].position = oneClosestVector.B;

            _dots[2].position = anotherClosestVector.A;
            _dots[3].position = anotherClosestVector.B;
        }

        //private IEnumerator DockPart(LetterPart part, float rotationAngle)
        //{
        //    isDocking = true;
        //    part.Body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        //    var quaternion = Quaternion.Euler(rotationAngle, 0f, 0f);
        //    var desiredRotation = part.transform.rotation.z + rotationAngle;

        //    while (!Mathf.Approximately(part.transform.rotation.z, desiredRotation))
        //    {
        //        var towards = Quaternion.RotateTowards(part.transform.rotation, quaternion, _dockSpeed * Time.deltaTime);
        //        part.Body.MoveRotation(towards);
        //        yield return null;
        //    }

        //    part.Body.constraints = RigidbodyConstraints2D.FreezePositionX
        //                          | RigidbodyConstraints2D.FreezePositionY
        //                          | RigidbodyConstraints2D.FreezeRotation;
        //    isDocking = false;
        //}

        private LetterPart GetNearestLetterPart(LetterPart currentPart)
        {
            var letter = SessionManager.Instance.LetterInstance;
            float maxDistance = float.MaxValue;
            LetterPart desiredPart = null;

            foreach (var suspect in letter.LetterParts)
            {
                if (suspect != currentPart)
                {
                    var currentDistance = GetDistanceBetween(suspect.transform.position, currentPart.transform.position);
                    if (currentDistance < maxDistance)
                    {
                        maxDistance = currentDistance;
                        desiredPart = suspect;
                    }
                }
            }

            return desiredPart;
        }

        private MathVector GetClosestVector(List<MathVector> vectors, Vector2 position)
        {
            MathVector desiredVector = null;

            float maxDistance = float.MaxValue;
            foreach (var vector in vectors)
            {
                var distance = GetDistanceBetween(vector.Middle, position);
                if (distance < maxDistance)
                {
                    maxDistance = distance;
                    desiredVector = vector;
                }

            }
            return desiredVector;
        }

        private float GetDistanceBetween(Vector3 one, Vector3 another)
        {
            var distanceVector = one - another;
            return Mathf.Sqrt(distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y);
        }

        private List<MathVector> GetMathVectors(PolygonCollider2D collider)
        {
            var points = collider.points;
            var result = new List<MathVector>();
            for (int i = 0, j = 1; i < points.Length; ++i, ++j)
            {
                if (j == points.Length)
                {
                    j = 0;
                }

                var onePosition = CalcWorldPointPosition(points[i].x, points[i].y, collider.transform);
                var anotherPosition = CalcWorldPointPosition(points[j].x, points[j].y, collider.transform);
                var mathVector = new MathVector
                {
                    A = onePosition,
                    B = anotherPosition,
                    Middle = CalcMiddleVector(onePosition, anotherPosition)
                };

                result.Add(mathVector);
            }
            return result;
        }

        private Vector2 CalcMiddleVector(Vector2 one, Vector2 another)
        {
            return new Vector2((one.x + another.x) / 2, (one.y + another.y) / 2);
        }

        private Vector2 CalcWorldPointPosition(float x, float y, Transform owner)
        {
            var pointLocalPosition = new Vector2(x, y);
            return owner.TransformPoint(pointLocalPosition);
        }

        private Vector2 CalcVectorCoords(MathVector vector)
        {
            return new Vector2(vector.B.x - vector.A.x, vector.B.y - vector.A.y);
        }

        private float CalcArcCosBetweenVectors(MathVector one, MathVector another)
        {
            var oneVectorCoords = CalcVectorCoords(one);
            var anotherVectorCoords = CalcVectorCoords(another);
            float numerator = oneVectorCoords.x * anotherVectorCoords.x + oneVectorCoords.y * anotherVectorCoords.y;
            float oneVectorLength = Mathf.Sqrt(oneVectorCoords.x * oneVectorCoords.x + oneVectorCoords.y * oneVectorCoords.y);
            float anotherVectorLength = Mathf.Sqrt(anotherVectorCoords.x * anotherVectorCoords.x + anotherVectorCoords.y * anotherVectorCoords.y);
            float denominator = oneVectorLength * anotherVectorLength;
            float angle = Mathf.Acos(numerator / denominator) * Mathf.Rad2Deg;
            return angle;
        }
    }
}