using LibChaiiLatte.Utils;

namespace SF.Common.Packet.ManualSerializable {
    public struct ServerStatePacket : INetSerializable {
        public ushort Tick { get; set; }
        public ushort LastProcessedAction { get; set; }

        public int CharacterStateCount { get; set; }
        public int StartState { get; set; }
        public CharacterPacket[] CharacterStates;

        public const int HeaderSize = sizeof(ushort) * 2;

        public void Serialize(NetDataWriter writer) {
            writer.Put(Tick);
            writer.Put(LastProcessedAction);

            for (int i = 0; i < CharacterStateCount; i++) {
                CharacterStates[StartState + i].Serialize(writer);
            }
        }

        public void Deserialize(NetDataReader reader) {
            Tick = reader.GetUShort();
            LastProcessedAction = reader.GetUShort();
            CharacterStateCount = reader.AvailableBytes / CharacterPacket.Size;
            
            if (CharacterStates == null || CharacterStates.Length < CharacterStateCount) {
                CharacterStates = new CharacterPacket[CharacterStateCount];
            }

            for (int i = 0; i < CharacterStateCount; i++) {
                CharacterStates[i].Deserialize(reader);
            }
        }
    }
}