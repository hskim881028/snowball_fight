using UnityEngine;

namespace SF.Joystick {
    public class JoystickController {
        private readonly JoystickView _view;

        public JoystickController(JoystickData data, JoystickView view) {
            Data = data;
            _view = view;
        }

        public JoystickData Data { get; private set; }

        public void OnDrag(Vector2 position) {
            Data = new JoystickData(_view.OnDrag(position));
        }

        public void OnPointerDown(Vector2 position) {
            _view.OnPointerDown(position);
            Data = new JoystickData(Vector2.zero);
        }

        public void OnPointerUp() {
            _view.OnPointerUp();
            Data = new JoystickData(Vector2.zero);
        }
    }
}