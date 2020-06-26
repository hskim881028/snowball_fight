using UnityEngine;

namespace hskim {
    public class CharacterVariableData {
        public Vector2 Position { get; private set; }
        public int HP { get; private set; }

        public CharacterVariableData(Vector2 position, int hp) {
            Position = position;
            HP = hp;
        }

        public void AddPosition(Vector2 value) {
            Position += value;
        }
    }
}