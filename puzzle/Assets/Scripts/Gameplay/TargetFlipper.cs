using Managers;
using System;
using UnityEngine;

namespace Gameplay
{
    public class TargetFlipper : MonoBehaviour
    {
        [SerializeField] private float _xAngle;
        [SerializeField] private float _yAngle;

        private LetterPart _lastLetterPart;
        public static Action TargetFlipEvent;

        private void OnEnable()
        {
            InputManager.TargetCaptureEvent += OnTargetCaptured;
        }

        private void OnDisable()
        {
            InputManager.TargetCaptureEvent -= OnTargetCaptured;
        }

        private void OnTargetCaptured(LetterPart last)
        {
            _lastLetterPart = last;
        }

        public void FlipTarget(bool xAxis)
        {
            if (_lastLetterPart == null)
            {
                return;
            }
            var currentEulerAngles = _lastLetterPart.transform.eulerAngles;
            if (xAxis)
            {
                currentEulerAngles.x += _xAngle;
            }
            else
            {
                currentEulerAngles.y += _yAngle;
            }
            _lastLetterPart.transform.eulerAngles = currentEulerAngles;
            TargetFlipEvent?.Invoke();
        }
    }
}
