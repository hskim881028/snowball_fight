using UnityEngine;

namespace SF.Network {
    public class ClientManager : NetworkCharacterManager<ClientCharacter> {
        public void UpdateLogic() {
            foreach (var character in Characters) {
                Debug.Log($"Id : {character.Value._data.Id}");
            }
        }
    }
}
