using Managers;
using UnityEngine;

namespace Gameplay
{
    public class TargetFlipper : MonoBehaviour
    {
        [SerializeField] private float _xAngle;
        [SerializeField] private float _yAngle;

        private InputManager _inputManager;
        
        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();
        }

        public void FlipTargetX()
        {
            var target = _inputManager.LastTarget;
            var currentEulerAngles = target.transform.eulerAngles;
            target.transform.eulerAngles = new Vector3(
                 currentEulerAngles.x + _xAngle,
                     currentEulerAngles.y,
                     currentEulerAngles.z
                );
        }

        public void FlipTargetY()
        {
            var target = _inputManager.LastTarget;
            var currentEulerAngles = target.transform.eulerAngles;
            target.transform.eulerAngles = new Vector3(
                currentEulerAngles.x,
                currentEulerAngles.y + _yAngle,
                currentEulerAngles.z
            );
        }
    }
}
