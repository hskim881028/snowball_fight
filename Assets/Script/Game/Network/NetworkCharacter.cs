using SF.Character;
using SF.Common.Util;

namespace SF.Network {
    public class NetworkCharacter {
        public readonly CharacterData _data;
        public readonly CharacterVariableData _variableData;
        public int Ping { get; set; }
        
        public NetworkCharacter(int id, string name) {
            _data = new CharacterData(id, name);
            _variableData = new CharacterVariableData(Gameconfig.StartingPoint[id]);
        }
    }
}
