using System;
using System.Net;
using System.Net.Sockets;
using LibChaiiLatte;
using LibChaiiLatte.Utils;
using SF.Common.Packet;
using SF.Common.Packet.AutoSerializable;
using SF.Common.Packet.ManualSerializable;
using SF.Common.Util;
using SF.Network;
using UnityEngine;
using Random = System.Random;

namespace SF.Service {
    public class ClientService : INetEventListener {
        private readonly ClientManager _clientManager;
        private readonly NetManager _netManager;
        private readonly NetPacketProcessor _packetProcessor;
        private readonly string _userName;
        private readonly NetDataWriter _writer;
        private ServerStatePacket _cachedServerState;
        private ushort _lastServerTick;

        private Action<DisconnectInfo> _onDisconnected;

        private NetPeer _peer;
        private int _ping;

        public ClientService(ActionService actionService, CharacterService characterService, Transform stage) {
            _clientManager = new ClientManager(actionService, characterService, stage);
            _cachedServerState = new ServerStatePacket();
            _writer = new NetDataWriter();
            _packetProcessor = new NetPacketProcessor();
            _packetProcessor.RegisterNestedType((writer, v) => writer.Put(v), reader => reader.GetVector2());
            _packetProcessor.RegisterNestedType<CharacterPacket>();
            _packetProcessor.SubscribeReusable<CharacterJoinedPacket>(OnPlayerJoined);
            _packetProcessor.SubscribeReusable<PlayerLeavedPacket>(OnPlayerLeaved);
            _packetProcessor.SubscribeReusable<JoinAcceptPacket>(OnJoinAccept);

            var rand = new Random();
            _userName = $"{Environment.MachineName} {rand.Next(100000)}";

            _netManager = new NetManager(this, true);
            _netManager.Start();

            LogicTimer = new LogicTimer(OnUpdateLogic);
        }

        public byte Id { get; private set; }

        public static LogicTimer LogicTimer { get; private set; }

        // 1 순위
        public void OnPeerConnected(NetPeer peer) {
            Debug.Log($"[Client] OnPeerConnected - EndPoint: {peer.EndPoint}");
            _peer = peer;
            SendPacket(new JoinPacket { UserName = _userName }, SendType.ReliableOrdered);
            LogicTimer.Start();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            // _characterManager.Clear();
            _peer = null;
            LogicTimer.Stop();
            Debug.Log($"[Client] OnPeerDisconnected - Reason : {disconnectInfo.Reason} ");

            _onDisconnected?.Invoke(disconnectInfo);
            _onDisconnected = null;
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
            Debug.Log($"[Client] NetworkError: {socketError}");
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint,
                                                NetPacketReader reader,
                                                UnconnectedMessageType messageType) {
            // do nothing
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
            _ping = latency;
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            request.Reject();
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, SendType sendType) {
            var packetType = reader.GetByte();
            if (packetType >= NetworkGeneral.PacketTypesCount) return;

            var pt = (PacketType) packetType;
            switch (pt) {
                case PacketType.ServerState:
                    _cachedServerState.Deserialize(reader);
                    OnServerState(); // <- 매번 서버에서 불러주고있음
                    break;
                case PacketType.Serialized:
                    _packetProcessor.ReadAllPackets(reader);
                    break;
                default:
                    Debug.Log($"[Client] OnNetworkReceive - Unhandled packet : {pt}");
                    break;
            }
        }

        public void Update() {
            _netManager.PollEvents();
            LogicTimer.Update();
        }

        private void OnUpdateLogic() {
        }

        public void Destroy() {
            _netManager.Stop();
        }

        private void OnServerState() {
            if (NetworkGeneral.SeqDiff(_cachedServerState.Tick, _lastServerTick) <= 0
            ) //skip duplicate or old because we received that packet unreliably
                return;

            _lastServerTick = _cachedServerState.Tick;
            _clientManager.UpdateLogic(_cachedServerState.CharacterStates);
        }

        private void OnPlayerJoined(CharacterJoinedPacket packet) {
            Debug.Log($"[Client] Player joined: {packet.UserName}");
            var clientCharacter = new ClientCharacter(packet.CharacterPacket.Id, packet.UserName);
            _clientManager.AddCharadcter(packet.CharacterPacket.Id, clientCharacter);
        }

        private void OnJoinAccept(JoinAcceptPacket packet) {
            Id = packet.Id;
            Debug.Log("[Client] OnJoinAccept : " + packet.Id);
            _lastServerTick = packet.ServerTick;
            var clientCharacter = new ClientCharacter(packet.Id, packet.UserName);
            _clientManager.AddCharadcter(packet.Id, clientCharacter);
        }

        private void OnPlayerLeaved(PlayerLeavedPacket packet) {
            _clientManager.RemoveCharacter(packet.Id);
        }

        public void Connect(string ip, Action<DisconnectInfo> onDisconnected) {
            _onDisconnected = onDisconnected;
            _netManager.Connect(ip, 10515, "EnterGame");
        }

        public void SendPacketSerializable<T>(PacketType type, T packet, SendType sendType) where T : INetSerializable {
            if (_peer == null) return;

            _writer.Reset();
            _writer.Put((byte) type);
            packet.Serialize(_writer);
            _peer.Send(_writer, sendType);
        }

        public void SendPacket<T>(T packet, SendType sendType) where T : class, new() {
            if (_peer == null) return;

            _writer.Reset();
            _writer.Put((byte) PacketType.Serialized);
            _packetProcessor.Write(_writer, packet); // 내부에서 시리얼라이즈
            _peer.Send(_writer, sendType);
        }
    }
}