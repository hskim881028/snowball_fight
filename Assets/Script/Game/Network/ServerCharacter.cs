using LibChaiiLatte;
using SF.Common.Packet.ManualSerializable;
using UnityEngine;

namespace SF.Network {
    public class ServerCharacter : NetworkCharacter {
        
        public ushort LastProcessedAction { get; private set; }
        public NetPeer Peer { get; }
        public CharacterPacket CharacterPacket { get; }

        public ServerCharacter(string name, NetPeer peer, Vector2 position) : base(peer.Id, name) {
            Peer = peer;
            Peer.Tag = this;
            CharacterPacket = new CharacterPacket {Id = (byte) peer.Id};
            _variableData.Position = position;
        }
    }
}