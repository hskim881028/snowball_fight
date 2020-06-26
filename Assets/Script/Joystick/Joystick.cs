using UnityEngine;

namespace hskim {
    public class Joystick {
        readonly JoystickView mView;
        public JoystickData Data { get; private set; }

        public Joystick(JoystickData data, JoystickView view) {
            Data = data;
            mView = view;
        }

        public void OnDrag(Vector2 position) {
            Data = new JoystickData(mView.OnDrag(position));
        }

        public void OnPointerDown(Vector2 position) {
            mView.OnPointerDown(position);
            Data = new JoystickData(Vector2.zero);
        }
        
        public void OnPointerUp() {
            mView.OnPointerUp();
            Data = new JoystickData(Vector2.zero);
        }
    }
}