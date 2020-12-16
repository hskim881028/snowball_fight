using LibChaiiLatte.Utils;
using SF.Common.Util;
using UnityEngine;

namespace SF.Common.Packet.ManualSerializable {
    public struct MovePacket : INetSerializable {
        public byte Id;
        public Vector2 Direction;
        public ushort ServerTick;
        
        public void Serialize(NetDataWriter writer) {
            writer.Put(Id);
            writer.Put(Direction);
            writer.Put(ServerTick);
        }

        public void Deserialize(NetDataReader reader) {
            Id = reader.GetByte();
            Direction = reader.GetVector2();
            ServerTick = reader.GetUShort();
        }
    }
}