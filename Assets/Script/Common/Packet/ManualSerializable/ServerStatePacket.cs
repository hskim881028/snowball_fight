using LibChaiiLatte.Utils;

namespace SF.Common.Packet.ManualSerializable {
    public struct ServerStatePacket : INetSerializable {
        public ushort Tick { get; private set; }
        public ushort LastProcessedAction { get; private set; }

        public int PlayerStateCount { get; set; }
        public int StartState { get; set; }
        public PlayerPacket[] Players;

        public const int HeaderSize = sizeof(ushort) * 2;

        public void Serialize(NetDataWriter writer) {
            writer.Put(Tick);
            writer.Put(LastProcessedAction);

            for (int i = 0; i < PlayerStateCount; i++) {
                Players[StartState + i].Serialize(writer);
            }
        }

        public void Deserialize(NetDataReader reader) {
            Tick = reader.GetUShort();
            LastProcessedAction = reader.GetUShort();
            PlayerStateCount = reader.AvailableBytes / PlayerPacket.Size;
            
            if (Players == null || Players.Length < PlayerStateCount) {
                Players = new PlayerPacket[PlayerStateCount];
            }

            for (int i = 0; i < PlayerStateCount; i++) {
                Players[i].Deserialize(reader);
            }
        }
    }
}