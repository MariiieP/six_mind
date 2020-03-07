using Gameplay;
using UnityEngine;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		private bool _isMoving = false;
		private LetterPart _target = null;
		private Vector3 _delta = new Vector2(0, 0);

		private void FixedUpdate()
		{
			if (Input.GetMouseButton(0))
			{
				var mousePosition = Input.mousePosition;
				var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
				var rayHit = Physics2D.Raycast(worldPosition, Vector2.zero);

				if (rayHit.transform != null)
				{
					GetTarget(rayHit);

					if (_target != null)
					{
						if (!_isMoving) // first time
						{
							_delta = worldPosition - _target.transform.position;
						}
						_isMoving = true;
					}
				}

				MoveTarget(worldPosition - _delta);
			}
			else
			{
				_target = null;
				_isMoving = false;
			}
		}

		private void MoveTarget(Vector2 position)
		{
			if (_isMoving && _target != null)
			{
				_target.Body.MovePosition(position);
			}
		}

		private void GetTarget(RaycastHit2D rayHit)
		{
			if (_target == null)
			{
				_target = rayHit.transform.GetComponent<LetterPart>();
			}
		}
	}
}
