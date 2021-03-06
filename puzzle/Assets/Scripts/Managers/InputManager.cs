﻿using Gameplay;
using System;
using UnityEngine;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		private bool _targetCaptured = false;
		private LetterPart _target = null;

		public static Action<LetterPart> TargetDropEvent;
		public static Action<LetterPart> TargetCaptureEvent;
		public static bool InputAccess { get; set; }

		private TargetMover _targetMover;
		private TargetRotator _targetRotator;

		private void Awake()
		{
			_targetMover = new TargetMover();
			_targetRotator = new TargetRotator();
			InputAccess = true;
		}

		private void Update()
		{
			if (!InputAccess)
			{
				return;
			}

#if UNITY_IPHONE || UNITY_ANDROID && !UNITY_EDITOR
			SmartPhoneCast();
#endif

#if UNITY_EDITOR

			EditorCast();
#endif
		}

		private void EditorCast()
		{
			var leftMouseButtonPressed = Input.GetMouseButton(0);
			var rightMouseButtonPressed = Input.GetMouseButton(1);

			if (leftMouseButtonPressed || rightMouseButtonPressed)
			{
				if (!_targetCaptured && !IsMouseMoving && !_targetMover.IsMoving && !_targetRotator.IsRotating)
				{
					CaptureTarget();
					if (_targetCaptured)
					{
						TargetCaptureEvent?.Invoke(_target);
					}
				}

				if (_targetCaptured && IsMouseMoving)
				{
					if (leftMouseButtonPressed)
					{
						_targetMover.Move(_target);
					}
					else if (rightMouseButtonPressed)
					{
						_targetRotator.Rotate(_target);
					}
				}
			}

			var leftMouseButtonUp = Input.GetMouseButtonUp(0);
			var rightMouseButtonUp = Input.GetMouseButtonUp(1);

			if ((leftMouseButtonUp || rightMouseButtonUp) && _target != null && (_targetRotator.IsRotating || _targetMover.IsMoving))
			{
				DropTarget();
			}
		}

		private void SmartPhoneCast()
		{
			var touches = Input.touches;

			if (touches.Length == 0)
			{
				return;
			}

			if (touches.Length > 0 && touches.Length <= 2)
			{
				if (!_targetCaptured && !IsMouseMoving && !_targetMover.IsMoving && !_targetRotator.IsRotating)
				{
					CaptureTarget();
					if (_targetCaptured)
					{
						TargetCaptureEvent?.Invoke(_target);
					}
				}

				if (_targetCaptured && IsMouseMoving)
				{
					if (touches.Length > 1)
					{
						_targetRotator.Rotate(_target);
					}
					else
					{
						_targetMover.Move(_target);
					}
				}
			}

			if (touches[0].phase == TouchPhase.Ended && _target != null && (_targetRotator.IsRotating || _targetMover.IsMoving))
			{
				DropTarget();
			}
		}

		public static Vector3 TouchWorldPosition => Camera.main.ScreenToWorldPoint(TouchPosition);

		public static Vector3 TouchPosition
		{
			get
			{
				Vector3 origin;

#if UNITY_EDITOR
				origin = Input.mousePosition;
#elif UNITY_IPHONE || UNITY_ANDROID
				origin = Input.GetTouch(0).position;
#endif
				return origin;
			}
		}

		private static bool IsMouseMoving
		{
			get
			{
				float xAxis = Input.GetAxis("Mouse X");
				float yAxis = Input.GetAxis("Mouse Y");
				var direction = new Vector2(xAxis, yAxis);
				return direction.magnitude != 0;
			}
		}

		private void CaptureTarget()
		{
			var rayHit = Physics2D.Raycast(TouchWorldPosition, Vector2.zero);
			if (rayHit.transform == null)
			{
				return;
			}

			var letterPart = rayHit.transform.GetComponent<LetterPart>();
			if (letterPart == null)
			{
				return;
			}

			_target = letterPart;
			_targetCaptured = true;
		}

		private void DropTarget()
		{
			_target.Body.constraints =
				RigidbodyConstraints2D.FreezePositionX
				| RigidbodyConstraints2D.FreezePositionY
				| RigidbodyConstraints2D.FreezeRotation;

			TargetDropEvent?.Invoke(_target);

			_target = null;
			_targetCaptured = false;
		}
	}
}
