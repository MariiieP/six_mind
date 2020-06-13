using Gameplay;
using Managers;
using UnityEngine.UI;

namespace UI
{
	public class SpriteFlicker
	{
		private const float _blur = 100f / 255f;
		private const float _light = 255f;

		public SpriteFlicker()
		{
			InputManager.TargetCaptureEvent += OnTargetCaptured;
			InputManager.TargetDropEvent += OnTargetDropped;
		}

		~SpriteFlicker()
		{
			InputManager.TargetCaptureEvent -= OnTargetCaptured;
			InputManager.TargetDropEvent -= OnTargetDropped;
		}

		private void OnTargetCaptured(LetterPart obj)
		{
			SetLetterPartsAlpha(obj, _blur, _light);
		}

		private void OnTargetDropped(LetterPart obj)
		{
			SetLetterPartsAlpha(obj, _light, _light);
		}

		private void SetLetterPartsAlpha(LetterPart _target, float blur, float light)
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
	}
}
