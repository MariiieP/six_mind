//using Managers;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Gameplay
//{
//    public class LetterPartDocker : MonoBehaviour
//    {
//        private class MathVector
//        {
//            public Vector2 A;
//            public Vector2 B;
//            public Vector2 Middle;
//        }

//        [SerializeField] private Transform[] _dots;
//        [SerializeField] private float _rotationMax;
//        [SerializeField] private float _distanceMax;
//        [SerializeField] private float _dockSpeed;

//        private void OnEnable()
//        {
//            InputManager.TargetDropEvent += OnTargetDropped;
//        }

//        private void OnDisable()
//        {
//            InputManager.TargetDropEvent -= OnTargetDropped;
//        }

//        private void OnTargetDropped(LetterPart one)
//        {
//            var colliders = one.GetOverlapCollider();

//            if (colliders.Length == 0)
//            {
//                Debug.Log("No colliders");
//                return;
//            }

//            LetterPart another = colliders[0].GetComponent<LetterPart>();

//            List<MathVector> oneVectors = GetMathVectors(one.PolygonColldier);
//            List<MathVector> anotherVectors = GetMathVectors(another.PolygonColldier);

//            var anotherIntersectionPoint = GetIntersection(one, another.transform.position);
//            var oneIntersectionPoint = GetIntersection(another, one.transform.position);

//            var oneClosestVector = GetClosestVectorToPoint(oneVectors, oneIntersectionPoint);
//            var anotherClosestVector = GetClosestVectorToPoint(anotherVectors, anotherIntersectionPoint);

//            var rotationAngle = CalcArcCosBetweenVectors(oneClosestVector, anotherClosestVector);

//            if (!float.IsNaN(rotationAngle))
//            {
//                if (rotationAngle > 90f)
//                {
//                    rotationAngle -= 180f;
//                }

//                if (rotationAngle < _rotationMax)
//                {
//                    rotationAngle = (one.Body.rotation > 0) ? rotationAngle : -rotationAngle;
//                    one.Body.SetRotation(one.Body.rotation + rotationAngle);
//                    Debug.Log("Done!");
//                }
//            }

//            Debug.Log("Rotation angle: " + rotationAngle);
//            _dots[0].position = oneClosestVector.A;
//            _dots[1].position = oneClosestVector.B;

//            _dots[2].position = anotherClosestVector.A;
//            _dots[3].position = anotherClosestVector.B;
//        }

//        private MathVector[] GetCommonVectors(List<MathVector> one, List<MathVector> another)
//        {
//            var result = new MathVector[2];
//            int idx = 0;
//            foreach (var oneVector in one)
//            {
//                var dot = oneVector.A;
//                foreach (var anotherVector in another)
//                {
//                    if (IsDotCommon(dot, anotherVector))
//                    {
//                        result[idx] = anotherVector;
//                        idx++;
//                    }
//                }
//            }

//            var letter = SessionManager.Instance.LetterInstance;
//            float maxDistance = float.MaxValue;
//            LetterPart desiredPart = null;

//            foreach (var suspect in letter.LetterParts)
//            {
//                if (suspect != currentPart)
//                {
//                    var distance = GetDistanceBetween(suspect.transform.position, currentPart.transform.position);
//                    if (distance < maxDistance)
//                    {
//                        maxDistance = distance;
//                        desiredPart = suspect;
//                    }
//                }
//            }

//            return desiredPart;
//        }

//        private Vector2 GetIntersection(LetterPart part, Vector3 target)
//        {
//            var from = part.transform.position;
//            var to = target - from;
//            var raycastResults = Physics2D.RaycastAll(from, to, _distanceMax);

//            Vector2 intersecton = Vector2.zero;

//            foreach (var result in raycastResults)
//            {
//                if (result.transform != part.transform)
//                {
//                    intersecton = result.point;
//                }
//            }

//            return intersecton;
//        }

//        private bool IsDotCommon(Vector3 dot, MathVector vector)
//        {
//            var minuend = (dot.x - vector.A.x) / (vector.B.x - vector.A.x);
//            var subtrahend = (dot.y - vector.A.y) / (vector.B.y - vector.A.y);
//            var pointToLine = Math.Abs(minuend - subtrahend);
//            const float raycastEpsilon = 0.3f;
//            return pointToLine < raycastEpsilon;
//        }

//        private MathVector GetClosestVectorToPoint(List<MathVector> vectors, Vector3 point)
//        {
//            MathVector desiredVector = null;

//            foreach (var vector in vectors)
//            {
//                var minuend = (point.x - vector.A.x) / (vector.B.x - vector.A.x);
//                var subtrahend = (point.y - vector.A.y) / (vector.B.y - vector.A.y);
//                var pointToLine = Math.Abs(minuend - subtrahend);
//                const float raycastEpsilon = 0.3f;
//                if (pointToLine < raycastEpsilon)
//                {
//                    desiredVector = vector;
//                }
//            }
//            return desiredVector;
//        }

//        private float GetDistanceBetween(Vector3 one, Vector3 another)
//        {
//            var distanceVector = one - another;
//            return Mathf.Sqrt(distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y);
//        }

//        private List<MathVector> GetMathVectors(PolygonCollider2D collider)
//        {
//            var points = collider.points;
//            var result = new List<MathVector>();
//            for (int i = 0, j = 1; i < points.Length; ++i, ++j)
//            {
//                if (j == points.Length)
//                {
//                    j = 0;
//                }

//                var onePosition = CalcWorldPointPosition(points[i].x, points[i].y, collider.transform);
//                var anotherPosition = CalcWorldPointPosition(points[j].x, points[j].y, collider.transform);
//                var mathVector = new MathVector
//                {
//                    A = onePosition,
//                    B = anotherPosition,
//                    Middle = CalcMiddleVector(onePosition, anotherPosition)
//                };

//                result.Add(mathVector);
//            }
//            return result;
//        }

//        private Vector2 CalcMiddleVector(Vector2 one, Vector2 another)
//        {
//            return new Vector2((one.x + another.x) / 2, (one.y + another.y) / 2);
//        }

//        private Vector2 CalcWorldPointPosition(float x, float y, Transform owner)
//        {
//            var pointLocalPosition = new Vector2(x, y);
//            return owner.TransformPoint(pointLocalPosition);
//        }

//        private Vector2 CalcVectorCoords(MathVector vector)
//        {
//            return new Vector2(vector.B.x - vector.A.x, vector.B.y - vector.A.y);
//        }

//        private float CalcArcCosBetweenVectors(MathVector one, MathVector another)
//        {
//            var oneVectorCoords = CalcVectorCoords(one);
//            var anotherVectorCoords = CalcVectorCoords(another);
//            float numerator = oneVectorCoords.x * anotherVectorCoords.x + oneVectorCoords.y * anotherVectorCoords.y;
//            float oneVectorLength = Mathf.Sqrt(oneVectorCoords.x * oneVectorCoords.x + oneVectorCoords.y * oneVectorCoords.y);
//            float anotherVectorLength = Mathf.Sqrt(anotherVectorCoords.x * anotherVectorCoords.x + anotherVectorCoords.y * anotherVectorCoords.y);
//            float denominator = oneVectorLength * anotherVectorLength;
//            float angle = Mathf.Acos(numerator / denominator) * Mathf.Rad2Deg;
//            return angle;
//        }
//    }
//}