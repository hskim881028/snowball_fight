using System.Net;
using System.Net.Sockets;
using LibChaiiLatte;
using LibChaiiLatte.Utils;
using SF.Common.Packet;
using SF.Common.Packet.AutoSerializable;
using SF.Common.Packet.ManualSerializable;
using SF.Common.Util;
using UnityEngine;

namespace SF {
    public class Server : INetEventListener {
        private NetManager _netManager;
        private NetPacketProcessor _packetProcessor;

        public const int MaxPlayers = 64;
        private LogicTimer _logicTimer;
        private readonly NetDataWriter _cachedWriter = new NetDataWriter();
        private ushort _serverTick;

        private ServerStatePacket _serverStatePacket;

        public ushort Tick => _serverTick;

        public Server() {
            _logicTimer = new LogicTimer(OnLogicUpdate);
            _packetProcessor = new NetPacketProcessor();

            _packetProcessor.RegisterNestedType((w, v) => w.Put(v), r => r.GetVector2());
            _packetProcessor.RegisterNestedType<PlayerPacket>();
            _packetProcessor.SubscribeReusable<JoinPacket, NetPeer>(OnJoinReceived);
            _netManager = new NetManager(this, true);
        }

        public void StartServer() {
            if (_netManager.IsRunning) {
                return;
            }

            _netManager.Start(10515);
            _logicTimer.Start();
        }

        public void Update() {
            _netManager.PollEvents();
            _logicTimer.Update();
        }

        private void OnLogicUpdate() {
        }

        public void Destroy() // todo : consider whether it is correct to doestroy on stage. 
        {
            _netManager.Stop();
            _logicTimer.Stop();
        }

        private void OnJoinReceived(JoinPacket packet, NetPeer peer) {
        }

        private NetDataWriter WriteSerializable<T>(PacketType type, T packet) where T : struct, INetSerializable {
            _cachedWriter.Reset();
            _cachedWriter.Put((byte) type);
            packet.Serialize(_cachedWriter);
            return _cachedWriter;
        }

        private NetDataWriter WritePacket<T>(T packet) where T : class, new() {
            _cachedWriter.Reset();
            _cachedWriter.Put((byte) PacketType.Serialized);
            _packetProcessor.Write(_cachedWriter, packet);
            return _cachedWriter;
        }

        public void OnPeerConnected(NetPeer peer) {
            Debug.Log($"[Server] Player connected: {peer.EndPoint}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            Debug.Log($"[Server] Player disconnected: {disconnectInfo.Reason}");

            if (peer.Tag != null)
            {
                // byte playerId = (byte)peer.Id;
                // if (_playerManager.RemovePlayer(playerId))
                // {
                //     var plp = new PlayerLeavedPacket { Id = (byte)peer.Id };
                //     _netManager.SendToAll(WritePacket(plp), DeliveryMethod.ReliableOrdered);
                // }
            }
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
            Debug.Log($"[Server] NetworkError: {socketError}");
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, SendType sendType) {
            byte packetType = reader.GetByte();
            if (packetType >= NetworkGeneral.PacketTypesCount)
                return;
            PacketType pt = (PacketType) packetType;
            switch (pt)
            {
                case PacketType.Movement:
                    break;
                case PacketType.Serialized:
                    _packetProcessor.ReadAllPackets(reader, peer);
                    break;
                default:
                    Debug.Log($"[Server] OnNetworkReceive - Unhandled packet : {pt}");
                    break;
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint,
                                                NetPacketReader reader,
                                                UnconnectedMessageType messageType) {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
            if (peer.Tag != null) {
                // var p = (ServerPlayer) peer.Tag;
                // p.Ping = latency;
            }
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            request.AcceptIfKey("EnterGame");
        }
    }
}