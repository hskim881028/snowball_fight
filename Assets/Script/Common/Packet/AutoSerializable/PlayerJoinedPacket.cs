using SF.Common.Packet.ManualSerializable;

namespace SF.Common.Packet.AutoSerializable {
    public class PlayerJoinedPacket {
        public string UserName { get; set; }
        public bool NewPlayer { get; set; }
        public ushort ServerTick { get; set; }
        public PlayerPacket Player { get; set; }
    }
}
