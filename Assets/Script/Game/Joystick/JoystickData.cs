using UnityEngine;

namespace SF.Joystick {
    public readonly struct JoystickData {
        public Vector2 Value { get; }

        public JoystickData(Vector2 value) {
            Value = value;
        }
    }
}