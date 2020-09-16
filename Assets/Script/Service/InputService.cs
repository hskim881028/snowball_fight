using hskim.Command;
using UnityEngine;
using UnityEngine.EventSystems;

namespace hskim {
    public class InputService : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
        private CommandService _commandService;
        private JoystickController _joystickController;

        private void Update() {
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

        public void OnDrag(PointerEventData eventData) {
            _joystickController.OnDrag(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData) {
            _joystickController.OnPointerDown(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData) {
            _joystickController.OnPointerUp();
        }

        public void Init(CommandService commandService, JoystickController joystickController) {
            _commandService = commandService;
            _joystickController = joystickController;
        }

        private void EnqueueCommand(int id, Vector2 delta) {
            _commandService.EnqueueCommand(new MoveCommand {mID = id, mDelta = delta * Time.deltaTime});
        }
    }
}