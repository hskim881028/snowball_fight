using System.Collections.Generic;
using System.Linq;
namespace hskim {
    public class CharacterService {
        readonly Dictionary<int, Character> mCharacters = new Dictionary<int, Character>();

        public Character GetCharacter(int id) => mCharacters[id];
        public void Add(int id, Character character) => mCharacters.Add(id, character);
    }
}