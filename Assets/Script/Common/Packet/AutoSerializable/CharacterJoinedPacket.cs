using SF.Common.Packet.ManualSerializable;

namespace SF.Common.Packet.AutoSerializable {
    public class CharacterJoinedPacket {
        public string UserName { get; set; }
        public bool NewCharacter { get; set; }
        public ushort ServerTick { get; set; }
        public CharacterPacket CharacterPacket { get; set; }
    }
}
