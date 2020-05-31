using Gameplay;
using UnityEngine;

namespace Managers
{
	public class TargetRotator
	{
		private const float _pinchTurnRatio = Mathf.PI / 2f;
		private const float _minTurnAngle = 0f;
		private float _turnAngleDelta = 0f;
		private Vector3 _onScreenPosition = new Vector2(0f, 0f);
		private float _angleDelta = 0f;

		public bool IsRotating = false;

		public TargetRotator()
		{
			InputManager.TargetDropped += OnTargetDropped;
		}

		~TargetRotator()
		{
			InputManager.TargetDropped -= OnTargetDropped;
		}

		public void Rotate(LetterPart target)
		{
#if UNITY_IPHONE || UNITY_ANDROID && !UNITY_EDITOR
			//SmartPhoneCast(target);
			EditorCast(target);
#endif

#if UNITY_EDITOR

			EditorCast(target);
#endif
		}

		private void EditorCast(LetterPart target)
		{
			var delta = InputManager.TouchPosition - _onScreenPosition;
			if (!IsRotating)
			{
				_onScreenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
				_angleDelta = (Mathf.Atan2(target.transform.right.y, target.transform.right.x) - Mathf.Atan2(delta.y, delta.x)) * Mathf.Rad2Deg;
				target.Body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
				IsRotating = true;
			}
			else
			{
				float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
				target.Body.MoveRotation(angle + _angleDelta);
			}
		}

		private void SmartPhoneCast(LetterPart target)
		{
			var desiredRotation = target.transform.rotation;

			CalcAngle();

			if (Mathf.Abs(_turnAngleDelta) > 0)
			{
				var rotationDeg = Vector3.zero;
				rotationDeg.z = -_turnAngleDelta;
				desiredRotation *= Quaternion.Euler(rotationDeg);
			}

			target.Body.MoveRotation(desiredRotation);
		}

		private void CalcAngle()
		{
			float turnAngle;
			_turnAngleDelta = 0f;

			var one = Input.GetTouch(0);
			var another = Input.GetTouch(1);

			if (one.phase == TouchPhase.Moved || another.phase == TouchPhase.Moved)
			{
				turnAngle = GetAngleBetween(one.position, another.position);
				float prevTurn = GetAngleBetween(one.position - one.deltaPosition, another.position - another.deltaPosition);
				_turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);

				if (Mathf.Abs(_turnAngleDelta) > _minTurnAngle)
				{
					_turnAngleDelta *= _pinchTurnRatio;
				}
				else
				{
					_turnAngleDelta = 0f;
				}
			}
		}

		private float GetAngleBetween(Vector2 one, Vector2 another)
		{
			var from = another - one;
			var to = new Vector2(1f, 0f);
			float result = Vector2.Angle(from, to);
			var cross = Vector3.Cross(from, to);

			if (cross.z > 0)
			{
				result = 360f - result;
			}

			return result;
		}

		private void OnTargetDropped(LetterPart obj)
		{
			IsRotating = false;
		}
	}
}
