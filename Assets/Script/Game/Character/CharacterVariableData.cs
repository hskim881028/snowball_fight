using UnityEngine;

namespace SF.Character {
    public class CharacterVariableData {
        // public int Hp { get; }

        public CharacterVariableData(Vector2 position) {
            Position = position;
            // Hp = hp;
        }

        public Vector2 Position { get; set; }
    }
}