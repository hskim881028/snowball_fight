using LibChaiiLatte;
using SF.Common.Packet;
using SF.Common.Packet.ManualSerializable;
using SF.Joystick;
using UnityEngine;
using UnityEngine.UI;

namespace SF.Service {
    public class InputService {
        private readonly ClientService _clientService;
        private readonly JoystickController _joystickController;
        private Image _inputPanel;

        public InputService(ClientService clientService, JoystickController joystickController) {
            _clientService = clientService;
            _joystickController = joystickController;
        }

        public void Update() {
            if (Input.touchCount > 0) EnqueueAction(Input.touches[0].deltaPosition);

            if (Input.GetMouseButtonDown(0)) _joystickController.OnPointerDown(Input.mousePosition);

            if (Input.GetMouseButton(0)) _joystickController.OnDrag(Input.mousePosition);

            if (Input.GetMouseButtonUp(0)) _joystickController.OnPointerUp();

            if (Input.GetKey(KeyCode.A)) EnqueueAction(Vector2.left);

            if (Input.GetKey(KeyCode.D)) EnqueueAction(Vector2.right);

            if (Input.GetKey(KeyCode.W)) EnqueueAction(Vector2.up);

            if (Input.GetKey(KeyCode.S)) EnqueueAction(Vector2.down);

            if (_joystickController.Data.Value.magnitude > 0) EnqueueAction(_joystickController.Data.Value);
        }

        private void EnqueueAction(Vector2 delta) {
            var p = new MovePacket {
                Id = _clientService.Id, Direction = delta * (3.0f * Time.deltaTime), ServerTick = 0
            };
            _clientService.SendPacketSerializable(PacketType.Movement, p, SendType.Unreliable);
        }
    }
}