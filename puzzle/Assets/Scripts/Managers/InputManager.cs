using Gameplay;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		private enum MouseState { SingleClick = 0, DoubleClick = 1 }

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

		private bool _isDoubleClick = false;
		private int _mouseClicks;
		private float _holdTimer;
		private float _holdTimerLimit = 0.19f;

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				CalcClickCount();

				Manage(_isDoubleClick? MouseState.DoubleClick : MouseState.SingleClick);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				DropTarget();
				_isDoubleClick = false;
			}
		}

		private void CalcClickCount()
		{
			if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
			{
				_mouseClicks++;
			}

			if (_mouseClicks >= 1 && _mouseClicks < 3)
			{
				_holdTimer += Time.deltaTime;

				if (_mouseClicks == 2 && _holdTimer - _holdTimerLimit < 0f)
				{
					_holdTimer = 0f;
					_mouseClicks = 0;
					_isDoubleClick = true;
				}

				if (_holdTimer > _holdTimerLimit)
				{
					_holdTimer = 0f;
					_mouseClicks = 0;
					_isDoubleClick = false;
				}
			}
		}

		private void Manage(MouseState state)
		{
			if (_targetCaptured)
			{
				if (state == MouseState.SingleClick)
				{
					MoveTarget();
				}
				else if (state == MouseState.DoubleClick)
				{
					RotateTarget();					
				}
			}
			else
			{
				CaptureTarget();
			}
		}

		private Vector3 MouseWorldPosition
		{
			get
			{
				var mousePosition = Input.mousePosition;
				return _camera.ScreenToWorldPoint(mousePosition);
			}
		}

		private void MoveTarget()
		{
			if (!_isMoving)
			{
				_positionDelta = MouseWorldPosition - _target.transform.position;
				_target.body.constraints = RigidbodyConstraints2D.FreezeRotation;
				_isMoving = true;

			}
			else
			{
				_target.body.MovePosition(MouseWorldPosition - _positionDelta);
			}
		}

		private void RotateTarget()
		{
			if (!_isRotating)
			{
				_onScreenPosition = _camera.WorldToScreenPoint(_target.transform.position);
				var delta = Input.mousePosition - _onScreenPosition;
				_angleDelta = (Mathf.Atan2(_target.transform.right.y, _target.transform.right.x) - Mathf.Atan2(delta.y, delta.x)) * Mathf.Rad2Deg;
				_target.body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
				_isRotating = true;
			}
			else
			{
				var delta = Input.mousePosition - _onScreenPosition;
				float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
				_target.body.MoveRotation(angle + _angleDelta);
			}
		}

		private void CaptureTarget()
		{
			var rayHit = Physics2D.Raycast(MouseWorldPosition, Vector2.zero);
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
				_target.body.constraints =
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
