using System.Collections.Generic;
using SF.Service;

namespace SF.Network {
    public class NetworkCharacterManager<T> where T : NetworkCharacter {
        public Dictionary<byte, T> Characters { get; } = new Dictionary<byte, T>();
        
        public virtual void AddCharadcter(byte id, T character) {
            Characters.Add(id, character);
        }

        public void RemoveCharacter(byte id) {
            Characters.Remove(id);
        }

        public T GetCharacter(byte id) {
            return Characters[id];
        }
    }
}
