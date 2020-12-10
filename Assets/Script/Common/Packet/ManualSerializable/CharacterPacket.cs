using LibChaiiLatte.Utils;
using SF.Common.Util;
using UnityEngine;

namespace SF.Common.Packet.ManualSerializable {
    public struct CharacterPacket : INetSerializable {
        public byte Id;
        public ushort Tick;
        public Vector2 Position;
        public float Rotation;

        public const int Size = 1 + 2 + 8 + 4; // Id + Tick + Position + Rotation 

        public void Serialize(NetDataWriter writer) {
            writer.Put(Id);
            writer.Put(Tick);
            writer.Put(Position);
            writer.Put(Rotation);
        }

        public void Deserialize(NetDataReader reader) {
            Id = reader.GetByte();
            Tick = reader.GetUShort();
            Position = reader.GetVector2();
            Rotation = reader.GetFloat();
        }
    }
}