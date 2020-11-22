using hskim.Action;
using UnityEngine;
using UnityEngine.UI;

namespace hskim {
    public class InputService {
        private Image _inputPanel;
        private readonly CommandService _commandService;
        private readonly JoystickController _joystickController;

        public InputService(CommandService commandService, JoystickController joystickController) {
            _commandService = commandService;
            _joystickController = joystickController;
        }
        
        public void Update() {
            if (Input.touchCount > 0) {
                EnqueueCommand(1, Input.touches[0].deltaPosition);
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
                EnqueueCommand(1, Vector2.left);
            }

            if (Input.GetKey(KeyCode.D)) {
                EnqueueCommand(1, Vector2.right);
            }

            if (Input.GetKey(KeyCode.W)) {
                EnqueueCommand(1, Vector2.up);
            }

            if (Input.GetKey(KeyCode.S)) {
                EnqueueCommand(1, Vector2.down);
            }

            if (_joystickController.Data.Value.magnitude > 0) {
                EnqueueCommand(1, _joystickController.Data.Value);
            }
        }
    
        private void EnqueueCommand(int id, Vector2 delta) {
            _commandService.EnqueueCommand(new MoveAction {mID = id, mDelta = delta * Time.deltaTime});
        }
    }
}
