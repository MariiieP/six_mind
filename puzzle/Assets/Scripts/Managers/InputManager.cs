using UnityEngine;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		private bool _isMoving = false;
		private Transform _targetTransform = null;

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				var mousePosition = Input.mousePosition;
				var currentWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
				var rayHit = Physics2D.Raycast(currentWorldPosition, Vector2.zero);

				if (rayHit.transform != null)
				{
					var go = rayHit.transform.gameObject;
					var component = go.GetComponent<MiniBlock>();

					if (component != null)
					{
						_targetTransform = go.transform;
						_isMoving = true;
					}
				}

				if (_isMoving && _targetTransform != null)
				{
					currentWorldPosition.z = 0;
					_targetTransform.position = currentWorldPosition;
				}
			}
			else
			{
				_isMoving = false;
			}
		}
	}
}
