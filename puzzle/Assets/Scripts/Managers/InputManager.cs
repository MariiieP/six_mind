using Gameplay;
using UnityEngine;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField] private Camera _camera;

		private bool _targetCaptured = false;
		private bool _isMoving = false;
		private LetterPart _target = null;
		private Vector3 _targetDelta = new Vector2(0, 0);

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				var mousePosition = Input.mousePosition;
				var worldPosition = _camera.ScreenToWorldPoint(mousePosition);

				if (_targetCaptured)
				{
					if (!_isMoving)
					{
						_targetDelta = worldPosition - _target.transform.position;
						_target.body.constraints = RigidbodyConstraints2D.FreezeRotation;
						_isMoving = true;

					}
					else
					{
						_target.body.MovePosition(worldPosition - _targetDelta);
					}
				}
				else
				{
					var rayHit = Physics2D.Raycast(worldPosition, Vector2.zero);
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
			}

			if (Input.GetMouseButtonUp(0))
			{
				if (_target != null)
				{
					_target.body.constraints =
						RigidbodyConstraints2D.FreezePositionX
						| RigidbodyConstraints2D.FreezePositionY
						| RigidbodyConstraints2D.FreezeRotation;
					_target = null;
					_isMoving = false;
					_targetCaptured = false;
				}
			}
		}
	}
}
