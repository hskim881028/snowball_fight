using LibChaiiLatte;
using SF.Character;
using SF.Common.Packet.ManualSerializable;
using UnityEngine;

namespace SF.Network {
    public class ServerCharacter : NetworkCharacter {
        public CharacterPacket CharacterPacket { get; set; }
        public ushort LastProcessedAction { get; private set; }
        public NetPeer Peer { get; }

        public CharacterPacket Packet { get; }

        public ServerCharacter(string name, NetPeer peer, Vector2 position) : base(new CharacterData(peer.Id, name)) {
            Peer = peer;
            Packet = new CharacterPacket {Id = (byte) peer.Id};
            _variableData.Position = position;
        }
    }
}