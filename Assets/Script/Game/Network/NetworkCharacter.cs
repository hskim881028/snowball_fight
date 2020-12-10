using SF.Character;
using UnityEngine;

namespace SF.Network {
    public class NetworkCharacter {
        public CharacterData _data;
        public CharacterVariableData _variableData;

        public NetworkCharacter(CharacterData data) {
            _data = data;
        }
    }
}
