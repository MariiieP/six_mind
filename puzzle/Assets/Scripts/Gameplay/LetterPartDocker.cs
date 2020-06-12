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
            public LetterPart Part;
        }

        [SerializeField] private Transform[] _dots;
        [SerializeField] private float _rotationMax;
        [SerializeField] private float _distanceMax;

        [SerializeField] private float _dockSpeed;

        private void OnEnable()
        {
            InputManager.TargetDropEvent += OnTargetDropped;
        }

        private void OnDisable()
        {
            InputManager.TargetDropEvent -= OnTargetDropped;
        }

        private void OnTargetDropped(LetterPart one)
        {
            var oneVectors = GetMathVectors(one);

            var anotherPart = GetClosestLetterPart(one, oneVectors);
            var anotherVectors = GetMathVectors(anotherPart);

            var closestVectors = GetClosestVectors(anotherVectors, oneVectors);
            var rotationAngle = CalcArcCosBetweenVectors(closestVectors[0], closestVectors[1]);

            _dots[0].position = closestVectors[0].A;
            _dots[1].position = closestVectors[0].B;

            _dots[2].position = closestVectors[1].A;
            _dots[3].position = closestVectors[1].B;

            if (!float.IsNaN(rotationAngle))
            {
                if (rotationAngle > _rotationMax)
                {
                    return;
                }

                one.Body.SetRotation(one.Body.rotation + rotationAngle);
            }
        }

        private LetterPart GetClosestLetterPart(LetterPart currentPart, List<MathVector> vectors)
        {
            var letterParts = SessionManager.Instance.LetterInstance.LetterParts;
            float maxDistance = float.MaxValue;
            LetterPart desiredPart = null;

            foreach (var suspect in letterParts)
            {
                if (suspect != currentPart)
                {
                    var suspectVectors = GetMathVectors(suspect);
                    foreach (var vector in vectors)
                    {
                        foreach (var suspectVector in suspectVectors)
                        {
                            var currentDistance = GetDistanceBetweenVectors(vector, suspectVector);
                            if (currentDistance < maxDistance)
                            {
                                maxDistance = currentDistance;
                                desiredPart = suspect;
                            }
                        }
                    }
                }
            }

            return desiredPart;
        }

        private float GetDistanceBetweenVectors(MathVector one, MathVector another)
        {
            float Distance(float x1, float y1, float x2, float y2, float x3, float y3)
            {
                if (x1 == x2)
                {
                    float temp = x1;
                    x1 = y1;
                    y1 = temp;

                    temp = x2;
                    x2 = y2;
                    y2 = temp;

                    temp = x3;
                    x3 = y3;
                    y3 = temp;
                }

                float k = (y1 - y2) / (x1 - x2);
                float d = y1 - k * x1;
                float xz = (x3 * x2 - x3 * x1 + y2 * y3 - y1 * y3 + y1 * d - y2 * d) / (k * y2 - k * y1 + x2 - x1);
                float dl = -1;

                if ((xz <= x2 && xz >= x1) || (xz <= x1 && xz >= x2))
                {
                    dl = Mathf.Sqrt((x3 - xz) * (x3 - xz) + (y3 - xz * k - d) * (y3 - xz * k - d));
                }
                return dl;
            }

            float t = -2;
            float s = -2;
            float min;

            float xa = one.A.x;
            float ya = one.A.y;

            float xb = one.B.x;
            float yb = one.B.y;

            float xc = another.A.x;
            float yc = another.A.y;

            float xd = another.B.x;
            float yd = another.B.y;

            float o = (xb - xa) * (-yd + yc) - (yb - ya) * (-xd + xc);
            float o1 = (xb - xa) * (yc - ya) - (yb - ya) * (xc - xa);
            float o2 = (-yd + yc) * (xc - xa) - (-xd + xc) * (yc - ya);

            if (o != 0)
            {
                t = o1 / o;
                s = o2 / o;
            }

            if ((t >= 0 && s >= 0) && (t <= 1 && s <= 1))
            {
                min = 0;
            }
            else
            {
                float dl1 = Distance(xa, ya, xb, yb, xc, yc);

                min = dl1;

                float dl2 = Distance(xa, ya, xb, yb, xd, yd);

                if ((dl2 < min && dl2 != -1) || min == -1)
                {
                    min = dl2;
                }
                float dl3 = Distance(xc, yc, xd, yd, xa, ya);

                if ((dl3 < min && dl3 != -1) || min == -1) 
                { 
                    min = dl3; 
                }

                float dl4 = Distance(xc, yc, xd, yd, xb, yb);

                if ((dl4 < min && dl4 != -1) || min == -1)
                {
                    min = dl4;
                }
                if (min == -1)
                {
                    dl1 = Mathf.Sqrt((xa - xc) * (xa - xc) + (ya - yc) * (ya - yc));

                    min = dl1;

                    dl2 = Mathf.Sqrt((xb - xd) * (xb - xd) + (yb - yd) * (yb - yd));

                    if (dl2 < min)
                    {
                        min = dl2;
                    }

                    dl3 = Mathf.Sqrt((xb - xc) * (xb - xc) + (yb - yc) * (yb - yc));

                    if (dl3 < min)
                    {
                        min = dl3;
                    }

                    dl4 = Mathf.Sqrt((xa - xd) * (xa - xd) + (ya - yd) * (ya - yd));

                    if (dl4 < min)
                    {
                        min = dl4;
                    }
                }
            }
            return min;
        }

        private MathVector[] GetClosestVectors(List<MathVector> oneVectors, List<MathVector> anotherVectors)
        {
            MathVector[] result = new MathVector[2];
            float maxValue = float.MaxValue;
            foreach (var vector in oneVectors)
            {
                foreach (var anotherVector in anotherVectors)
                {
                    float distance = GetDistanceBetweenVectors(vector, anotherVector);
                    if (distance <= maxValue)
                    {
                        maxValue = distance;
                        result[0] = vector;
                        result[1] = anotherVector;
                    }
                }
            }
            return result;
        }

        private List<MathVector> GetMathVectors(LetterPart part)
        {
            var collider = part.PolygonColldier;
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
                    Middle = CalcMiddleVector(onePosition, anotherPosition),
                    Part = part,
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

            if (angle > 90f)
            {
                angle -= 180f;
            }

            return Mathf.Abs(angle);
        }
    }
}