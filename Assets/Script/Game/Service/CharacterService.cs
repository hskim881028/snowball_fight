using System.Collections.Generic;

namespace hskim {
    public class CharacterService {
        private readonly Dictionary<int, Character> _characters = new Dictionary<int, Character>();

        public Character GetCharacter(int id) {
            return _characters[id];
        }

        public void Add(int id, Character character) {
            _characters.Add(id, character);
        }
    }
}