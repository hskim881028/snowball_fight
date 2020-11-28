using System.Collections.Generic;

namespace SF.Service {
    public class CharacterService {
        private readonly Dictionary<int, Character.Character> _characters = new Dictionary<int, Character.Character>();

        public Character.Character GetCharacter(int id) {
            return _characters[id];
        }

        public void Add(int id, Character.Character character) {
            _characters.Add(id, character);
        }
    }
}
