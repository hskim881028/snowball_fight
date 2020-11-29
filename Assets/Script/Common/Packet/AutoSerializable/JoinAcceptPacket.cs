namespace SF.Common.Packet.AutoSerializable {
    public class JoinAcceptPacket {
        public byte Id { get; set; }
        public ushort ServerTick { get; set; }
    }
}
