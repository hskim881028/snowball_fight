using System.Collections.Generic;
using UnityEngine;

namespace SF.Network {
    public class NetworkCharacterManager<T> where T : NetworkCharacter {
        public Dictionary<byte, T> Characters { get; } = new Dictionary<byte, T>();
        
        public void AddCharadcter(byte id, T character) {
            Characters.Add(id, character);
        }
    }
}
