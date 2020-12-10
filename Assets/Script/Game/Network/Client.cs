using System;
using System.Net;
using System.Net.Sockets;
using LibChaiiLatte;
using LibChaiiLatte.Utils;
using SF.Common.Packet;
using SF.Common.Packet.AutoSerializable;
using SF.Common.Packet.ManualSerializable;
using SF.Common.Util;
using UnityEngine;
using Random = System.Random;

namespace SF.Network {
    public class Client : INetEventListener {
        private ServerStatePacket _cachedServerState;
        private ClientManager _clientManager;
        private readonly NetDataWriter _writer;
        private readonly NetPacketProcessor _packetProcessor;
        private readonly NetManager _netManager;
        private readonly string _userName;
        
        private Action<DisconnectInfo> _onDisconnected;

        private NetPeer _server;
        private int _ping;

        public static LogicTimer LogicTimer { get; private set; }

        public Client() {
            _cachedServerState = new ServerStatePacket();
            _writer = new NetDataWriter();
            _packetProcessor = new NetPacketProcessor();
            _packetProcessor.RegisterNestedType((writer, v) => writer.Put(v), reader => reader.GetVector2());
            _packetProcessor.RegisterNestedType<CharacterPacket>();
            _packetProcessor.SubscribeReusable<PlayerJoinedPacket>(OnPlayerJoined);
            _packetProcessor.SubscribeReusable<PlayerLeavedPacket>(OnPlayerLeaved);
            _packetProcessor.SubscribeReusable<JoinAcceptPacket>(OnJoinAccept);

            Random rand = new Random();
            _userName = $"{Environment.MachineName} {rand.Next(100000)}";

            _netManager = new NetManager(this, true);
            _netManager.Start();
            
            LogicTimer = new LogicTimer(OnUpdateLogic);
        }

        public void Update() {
            _netManager.PollEvents();
            LogicTimer.Update();
        }
        
        private void OnUpdateLogic()
        {
            _clientManager.UpdateLogic();
        }

        public void Destroy() {
            _netManager.Stop();
        }

        private void OnServerState() {
            // todo : 여기서 클라이언트 캐릭터 매니저 불러주고
            // 매니저에서 리모트 캐릭터들 정보 동기화 -> 뷰 좌표 갱신 끝
        }
        
        private void OnPlayerJoined(PlayerJoinedPacket packet) {
            Debug.Log($"[Client] Player joined: {packet.UserName}");
            // var remotePlayer = new RemotePlayer(_playerManager, packet.UserName, packet);
            // var view = RemotePlayerView.Create(_remotePlayerViewPrefab, remotePlayer);
            // _playerManager.AddPlayer(remotePlayer, view);
        }

        private void OnPlayerLeaved(PlayerLeavedPacket packet) {
            // var player = _playerManager.RemovePlayer(packet.Id);
            // if(player != null)
            //     Debug.Log($"[Client] _playerManager : {packet.Id} - {player.Name}");
        }

        private void OnJoinAccept(JoinAcceptPacket packet) {
            Debug.Log("[Client] OnJoinAccept : " + packet.Id);
            // _lastServerTick = packet.ServerTick;
            // var clientPlayer = new ClientPlayer(this, _playerManager, _userName, packet.Id);
            // var view = ClientPlayerView.Create(_clientPlayerViewPrefab, clientPlayer);
            // _playerManager.AddClientPlayer(clientPlayer, view);
        }

        public void Connect(string ip, Action<DisconnectInfo> onDisconnected)
        {
            _onDisconnected = onDisconnected;
            _netManager.Connect(ip, 10515, "EnterGame");
        }
        
        public void SendPacketSerializable<T>(PacketType type, T packet, SendType sendType) where T : INetSerializable {
            if (_server == null) {
                return;
            }

            _writer.Reset();
            _writer.Put((byte) type);
            packet.Serialize(_writer);
            _server.Send(_writer, sendType);
        }

        public void SendPacket<T>(T packet, SendType sendType) where T : class, new() {
            if (_server == null) {
                return;
            }

            _writer.Reset();
            _writer.Put((byte) PacketType.Serialized);
            _packetProcessor.Write(_writer, packet); // 내부에서 시리얼라이즈
            _server.Send(_writer, sendType);
        }

        public void OnPeerConnected(NetPeer peer) {
            Debug.Log($"[Client] OnPeerConnected - EndPoint: {peer.EndPoint}");
            _server = peer;
            SendPacket(new JoinPacket {UserName = _userName}, SendType.ReliableOrdered);
            LogicTimer.Start();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            // _playerManager.Clear();
            _server = null;
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
            byte packetType = reader.GetByte();
            if (packetType >= NetworkGeneral.PacketTypesCount) {
                return;
            }

            PacketType pt = (PacketType) packetType;
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
    }
}