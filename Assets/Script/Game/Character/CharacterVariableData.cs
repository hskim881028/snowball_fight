using UnityEngine;

namespace SF.Character {
    public class CharacterVariableData {
        public Vector2 Position { get; set; }
        // public int Hp { get; }

        public CharacterVariableData(Vector2 position) {
            Position = position;
            // Hp = hp;
        }

        public void AddPosition(Vector2 value) {
            Position += value;
        }
    }
}