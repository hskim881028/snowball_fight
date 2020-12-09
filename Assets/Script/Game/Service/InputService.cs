using SF.Action;
using SF.Joystick;
using UnityEngine;
using UnityEngine.UI;

namespace SF.Service {
    public class InputService {
        private Image _inputPanel;
        private readonly ActionService _actionService;
        private readonly JoystickController _joystickController;

        public InputService(ActionService actionService, JoystickController joystickController) {
            _actionService = actionService;
            _joystickController = joystickController;
        }
        
        public void Update() {
            if (Input.touchCount > 0) {
                EnqueueAction(1, Input.touches[0].deltaPosition);
            }

            if (Input.GetMouseButtonDown(0)) {
                _joystickController.OnPointerDown(Input.mousePosition);
            }
            
            if (Input.GetMouseButton(0)) {
                _joystickController.OnDrag(Input.mousePosition);
            }
            
            if (Input.GetMouseButtonUp(0)) {
                _joystickController.OnPointerUp();
            }
            
            if (Input.GetKey(KeyCode.A)) {
                EnqueueAction(1, Vector2.left);
            }

            if (Input.GetKey(KeyCode.D)) {
                EnqueueAction(1, Vector2.right);
            }

            if (Input.GetKey(KeyCode.W)) {
                EnqueueAction(1, Vector2.up);
            }

            if (Input.GetKey(KeyCode.S)) {
                EnqueueAction(1, Vector2.down);
            }

            if (_joystickController.Data.Value.magnitude > 0) {
                EnqueueAction(1, _joystickController.Data.Value);
            }
        }
    
        private void EnqueueAction(int id, Vector2 delta) {
            _actionService.EnqueueAction(new MoveAction {mID = id, mDelta = delta * Time.deltaTime});
        }
    }
}
