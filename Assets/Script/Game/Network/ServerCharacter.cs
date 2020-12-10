using LibChaiiLatte;
using SF.Common.Packet.ManualSerializable;

namespace SF.Network {
    public class ServerCharacter : NetworkCharacter{
        public CharacterPacket CharacterPacket { get; set; }
        public ushort LastProcessedAction { get; private set; }
        public NetPeer Peer { get;}
        
        private CharacterPacket _packet;

        public ServerCharacter(string name, NetPeer peer) {
            Peer = peer;
            _packet = new CharacterPacket {Id = (byte) peer.Id};
        }
    }
}