using System;
using SF.Common.Packet;

namespace SF.Common.Util {
    public static class NetworkGeneral {
        public static readonly int PacketTypesCount = Enum.GetValues(typeof(PacketType)).Length;

        public const int MaxGameSequence = 1024;
        private const int HalfMaxGameSequence = 1024 / 2;

        public static int SeqDiff(int a, int b) {
            return Diff(a, b, HalfMaxGameSequence);
        }

        private static int Diff(int a, int b, int halfMax) {
            return (a - b + halfMax * 3) % (halfMax * 2) - halfMax;
        } 
    }
}
