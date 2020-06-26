using UnityEngine;

namespace hskim {
    public readonly struct JoystickData {
        public Vector2 Value { get; }
        
        public JoystickData(Vector2 value) {
            Value = value;
        }
    }
}