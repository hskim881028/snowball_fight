using UnityEngine;

namespace SF.Character {
    public class CharacterView : MonoBehaviour {
        public void SetPosition(Vector2 position) {
            transform.position = position;
        }
    }
}