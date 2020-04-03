using Gameplay;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
	public class InputManager : MonoBehaviour
	{
		private enum MouseButtons { Left = 0, Right = 1 }

		[SerializeField] private Camera _camera;
		[SerializeField] private Button[] _flipButtons;
		
		private bool _targetCaptured = false;
		private bool _isMoving = false;
		private bool _isRotating = false;
		private LetterPart _target = null;
		private Vector3 _positionDelta = new Vector2(0, 0);
		private Vector3 _onScreenPosition = new Vector2(0, 0);
		private float _angleDelta = 0f;

		public static Action TargetDropped;
		[HideInInspector] public LetterPart LastTarget;
		
		
		private void Update()
		{
			if (Input.GetMouseButton((int)MouseButtons.Left))
			{
				Manage(MouseButtons.Left);
			}

			if (Input.GetMouseButton((int)MouseButtons.Right))
			{
				Manage(MouseButtons.Right);
			}

			if (Input.GetMouseButtonUp((int)MouseButtons.Left) 
				|| Input.GetMouseButtonUp((int)MouseButtons.Right))
			{
				DropTarget();
				TargetDropped?.Invoke();
			}
		}

		private void Manage(MouseButtons button)
		{
			if (_targetCaptured)
			{
				if (button == MouseButtons.Left)
				{
					MoveTarget();
				}
				else if (button == MouseButtons.Right)
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
				_target.body.MoveRotation(angle);
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
				LastTarget = _target;
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
