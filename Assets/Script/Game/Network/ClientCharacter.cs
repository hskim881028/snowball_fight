using SF.Common.Packet.AutoSerializable;

namespace SF.Network {
    public class ClientCharacter : NetworkCharacter {
        public ClientCharacter(int id, string userName) : base(id, userName) {
        }
    }
}