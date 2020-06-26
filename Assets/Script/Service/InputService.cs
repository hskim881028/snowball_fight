using hskim.Command;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace hskim {
    public class InputService : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
        CommandService mCommandService;
        Joystick mJoystick;
        
        public void Init(CommandService commandService, Joystick joystick) {
            mCommandService = commandService;
            mJoystick = joystick;
        }
        
        public void OnDrag(PointerEventData eventData) {
            mJoystick.OnDrag(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData) {
            mJoystick.OnPointerDown(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData) {
            mJoystick.OnPointerUp();
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

            if (mJoystick.Data.Value.magnitude > 0) {
                EnqueueCommand(1, mJoystick.Data.Value);
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