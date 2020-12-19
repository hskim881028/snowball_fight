using SF.Character;
using SF.Common.Packet.ManualSerializable;
using UnityEngine;

namespace SF.Network {
    public class NetworkCharacter {
        public readonly CharacterData _data;
        public readonly CharacterVariableData _variableData;
        public int Ping { get; set; }
        
        public NetworkCharacter(int id, string name) {
            _data = new CharacterData(id, name);
            _variableData = new CharacterVariableData(Vector3.zero);
        }
    }
}
