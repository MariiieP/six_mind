using Gameplay;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		private enum InputState { Moving = 0, Rotating = 1 }

		[SerializeField] private Camera _camera;
		[SerializeField] private Button[] _flipButtons;
		
		private bool _targetCaptured = false;
		private bool _isMoving = false;
		private bool _isRotating = false;
		private LetterPart _target = null;
		private Vector3 _positionDelta = new Vector2(0, 0);
		private Vector3 _onScreenPosition = new Vector2(0, 0);
		private float _angleDelta = 0f;

		public static Action<LetterPart> TargetDropped;

		private void Update()
		{
#if UNITY_IPHONE || UNITY_ANDROID && !UNITY_EDITOR
			var touches = Input.touches;

			if (touches.Length == 0)
			{
				return;
			}

			if (touches.Length == 1)
			{
				Manage(InputState.Moving);
			}
			else if (touches.Length == 2)
			{
				Manage(InputState.Rotating);
			}

			if ((_isRotating || _isMoving) && touches[0].phase == TouchPhase.Ended)
			{
				DropTarget();
				SetLetterPartsAlpha(255f, 100f / 255f);
			}
#endif
#if UNITY_EDITOR
			if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
			{
				Manage(InputState.Moving);
			}
			else if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
			{
				Manage(InputState.Rotating);
			}

			if (Input.GetMouseButtonUp(0) && (_isRotating || _isMoving))
			{
				DropTarget();
				SetLetterPartsAlpha(255f, 100f / 255f);
			}
#endif
		}

		private void Manage(InputState state)
		{
			if (_targetCaptured && IsMouseMoving)
			{
				if (state == InputState.Moving)
				{
					MoveTarget();
				}
				else if (state == InputState.Rotating)
				{
					RotateTarget();
				}
			}
			else
			{
				if (!_isMoving && !_isRotating)
				{
					CaptureTarget();
					if (_targetCaptured)
					{
						SetLetterPartsAlpha(100f / 255f, 255f);
					}
				}
			}
		}

		private Vector3 TouchWorldPosition => _camera.ScreenToWorldPoint(TouchPosition);

		private Vector3 TouchPosition
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

		private bool IsMouseMoving
		{
			get
			{
				float xAxis = Input.GetAxis("Mouse X");
				float yAxis = Input.GetAxis("Mouse Y");
				var direction = new Vector2(xAxis, yAxis);
				return direction.magnitude != 0;
			}
		}

		private void MoveTarget()
		{
			if (!_isMoving)
			{
				_positionDelta = TouchWorldPosition - _target.transform.position;
				_target.Body.constraints = RigidbodyConstraints2D.FreezeRotation;
				_isMoving = true;

			}
			else
			{
				_target.Body.MovePosition(TouchWorldPosition - _positionDelta);
			}
		}

		private void RotateTarget()
		{
			var delta = TouchPosition - _onScreenPosition; // ?
			if (!_isRotating)
			{
				_onScreenPosition = _camera.WorldToScreenPoint(_target.transform.position);
				_angleDelta = (Mathf.Atan2(_target.transform.right.y, _target.transform.right.x) - Mathf.Atan2(delta.y, delta.x)) * Mathf.Rad2Deg;
				_target.Body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
				_isRotating = true;
			}
			else
			{
				float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
				_target.Body.MoveRotation(angle + _angleDelta);
			}
		}

		private void SetLetterPartsAlpha(float blur, float light)
		{
			var letterParts = SessionManager.Instance.LetterInstance.LetterParts;
			foreach (var part in letterParts)
			{
				if (part != _target)
				{
					part.SetSpriteAlpha(blur);
				}
				else
				{
					part.SetSpriteAlpha(light);
				}
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
			if (_target != null)
			{
				_target.Body.constraints =
					RigidbodyConstraints2D.FreezePositionX
					| RigidbodyConstraints2D.FreezePositionY
					| RigidbodyConstraints2D.FreezeRotation;
				TargetDropped?.Invoke(_target);
				_target = null;
				_isMoving = false;
				_isRotating = false;
				_targetCaptured = false;
				foreach (var flipButton in _flipButtons)
				{
					flipButton.interactable = true;
				}
			}
		}
	}
}
