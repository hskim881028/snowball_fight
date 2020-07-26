using hskim.Command;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace hskim {
    public class InputService : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
        CommandService mCommandService;
        JoystickController mJoystickController;
        
        public void Init(CommandService commandService, JoystickController joystickController) {
            mCommandService = commandService;
            mJoystickController = joystickController;
        }
        
        public void OnDrag(PointerEventData eventData) {
            mJoystickController.OnDrag(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData) {
            mJoystickController.OnPointerDown(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData) {
            mJoystickController.OnPointerUp();
        }
        
        void Update() {
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

            if (mJoystickController.Data.Value.magnitude > 0) {
                EnqueueCommand(1, mJoystickController.Data.Value);
            }
        }

        void EnqueueCommand(int id, Vector2 delta) {
            mCommandService.EnqueueCommand(new MoveCommand() {
                id = id, 
                delta = delta * Time.deltaTime
            });
        }
    }
}