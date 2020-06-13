using Gameplay;
using System;
using UnityEngine;

namespace Managers
{
	public class TargetMover
	{
		public bool IsMoving = false;
		private Vector3 _positionDelta = Vector3.zero;

		public TargetMover()
		{
			InputManager.TargetDropEvent += OnTargetDropped;
		}

		~TargetMover()
		{
			InputManager.TargetDropEvent -= OnTargetDropped;
		}

		public void Move(LetterPart target)
		{
			var touchWorldPosition = InputManager.TouchWorldPosition;

			if (!IsMoving)
			{
				_positionDelta = touchWorldPosition - target.transform.position;
				target.Body.constraints = RigidbodyConstraints2D.FreezeRotation;
				IsMoving = true;

			}
			else
			{
				target.Body.MovePosition(touchWorldPosition - _positionDelta);
			}
		}

		private void OnTargetDropped(LetterPart obj)
		{
			IsMoving = false;
		}
	}
}
