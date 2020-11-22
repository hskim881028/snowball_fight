using UnityEngine;

namespace hskim {
    public class CharacterVariableData {
        public CharacterVariableData(Vector2 position, int hp) {
            Position = position;
            Hp = hp;
        }

        public Vector2 Position { get; private set; }
        public int Hp { get; }

        public void AddPosition(Vector2 value) {
            Position += value;
        }
    }
}