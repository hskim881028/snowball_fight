using System.Collections.Generic;
using SF.Action;
using SF.Common.Packet.ManualSerializable;
using SF.Service;
using UnityEngine;

namespace SF.Network {
    public class ClientManager : NetworkCharacterManager<ClientCharacter> {
        private readonly ActionService _actionService;
        private readonly CharacterService _characterService;
        private readonly Transform _stage;

        public ClientManager(ActionService actionService, CharacterService characterService, Transform stage) {
            _actionService = actionService;
            _characterService = characterService;
            _stage = stage;
        }

        public override void AddCharadcter(byte id, ClientCharacter clientCharacter) {
            base.AddCharadcter(id, clientCharacter);
            var character = PrefabLoader.LoadCharacter(id, _stage);
            _characterService.Add(id, character);
        }

        public void UpdateLogic(IEnumerable<CharacterPacket> characterPackets) {
            
            foreach (var character in characterPackets) {
                _actionService.EnqueueAction(new MoveAction {
                    Id = character.Id, Position = character.Position
                });
                Debug.Log($"Id : {character.Id} - Pos : {character.Position}");
            }
        }
    }
}