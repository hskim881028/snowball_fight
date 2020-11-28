using LibChaiiLatte.Utils;
using SF.Common.Util;
using UnityEngine;

namespace SF.Common.Packet {
    public struct MovePacket : INetSerializable {
        public byte PlayerId;
        public Vector2 Direction;
        public ushort ServerTick;
        
        public void Serialize(NetDataWriter writer) {
            writer.Put(PlayerId);
            writer.Put(Direction);
            writer.Put(ServerTick);
        }

        public void Deserialize(NetDataReader reader) {
            PlayerId = reader.GetByte();
            Direction = reader.GetVector2();
            ServerTick = reader.GetUShort();
        }
    }
}