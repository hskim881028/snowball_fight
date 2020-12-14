using System.Linq;
using System.Net;
using System.Net.Sockets;
using LibChaiiLatte;
using LibChaiiLatte.Utils;
using SF.Common.Packet;
using SF.Common.Packet.AutoSerializable;
using SF.Common.Packet.ManualSerializable;
using SF.Common.Util;
using UnityEngine;

namespace SF.Network {
    public class Server : INetEventListener {
        private NetManager _netManager;
        private NetPacketProcessor _packetProcessor;

        public const int MaxPlayers = 64;
        private LogicTimer _logicTimer;
        private readonly NetDataWriter _cachedWriter = new NetDataWriter();
        private ushort _serverTick;

        private ServerStatePacket _serverStatePacket;

        private readonly ServerManager _serverManager = new ServerManager();

        public ushort Tick => _serverTick;

        public Server() {
            _logicTimer = new LogicTimer(OnUpdateLogic);
            _packetProcessor = new NetPacketProcessor();

            _packetProcessor.RegisterNestedType((w, v) => w.Put(v), r => r.GetVector2());
            _packetProcessor.RegisterNestedType<CharacterPacket>();
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

        private void OnUpdateLogic() {
            _serverTick = (ushort) ((_serverTick + 1) % NetworkGeneral.MaxGameSequence);

            if (_serverTick % 2 == 0) {
                _serverStatePacket.Tick = _serverTick;
                _serverStatePacket.CharacterStates = 
                    _serverManager.Characters.
                                   Select(x => x.Value.CharacterPacket)
                                   .ToArray();

                foreach (var pair in _serverManager.Characters) {
                    var serverCharacter = pair.Value;
                    int statesMax = serverCharacter.Peer.GetMaxSinglePacketSize(SendType.Unreliable) -
                                    ServerStatePacket.HeaderSize;
                    statesMax /= CharacterPacket.Size;
                    
                    

                    for (int s = 0; s < (_serverManager.Characters.Count - 1) / statesMax + 1; s++) {
                        _serverStatePacket.LastProcessedAction = serverCharacter.LastProcessedAction;
                        _serverStatePacket.CharacterStateCount = _serverManager.Characters.Count;
                        _serverStatePacket.StartState = s * statesMax;
                        serverCharacter.Peer.Send(WriteSerializable(PacketType.ServerState, _serverStatePacket),
                                                  SendType.Unreliable);
                    }
                }
            }
        }

        public void Destroy() // todo : consider whether it is correct to doestroy on stage. 
        {
            _netManager.Stop();
            _logicTimer.Stop();
        }

        private void OnJoinReceived(JoinPacket packet, NetPeer peer) {
            Debug.Log("[S] Join packet received: " + packet.UserName);
            var serverCharacter = new ServerCharacter(packet.UserName, peer, Vector2.zero);
            _serverManager.AddCharadcter((byte)peer.Id, serverCharacter);

            //Send join accept
            var ja = new JoinAcceptPacket {Id = (byte)peer.Id, ServerTick = _serverTick};
            peer.Send(WritePacket(ja), SendType.ReliableOrdered);

            // 들어온 친구한테 기존 정보 다 보내줌
            var pj = new PlayerJoinedPacket 
            {
                UserName = packet.UserName,
                NewPlayer = true,
                CharacterPacket = serverCharacter.Packet,
                ServerTick = _serverTick
            };
            _netManager.SendToAll(WritePacket(pj), SendType.ReliableOrdered, peer);

            // 이전 유저들에게 정보 다 보내줌  
            pj.NewPlayer = false;
            foreach (var pair in _serverManager.Characters.Where(pair => pair.Key != (byte) peer.Id)) {
                pj.UserName = pair.Value._data.Name;
                pj.CharacterPacket = pair.Value.Packet;
                peer.Send(WritePacket(pj), SendType.ReliableOrdered);
            }
        }

        private MovePacket _movePacket;

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

        private void OnInputReceived(NetPacketReader reader, NetPeer peer) {
            // if (peer.Tag == null)
            //     return;
            //
            // _movePacket.Deserialize(reader);
            // var player = (Serc) peer.Tag;
            //
            // bool antilagApplied = _serverManager.EnableAntilag(player);
            // player.ApplyInput(_movePacket, LogicTimer.FixedDelta);
            // if(antilagApplied)
            //     _serverManager.DisableAntilag();
        }

        public void OnPeerConnected(NetPeer peer) {
            Debug.Log($"[Server] Player connected: {peer.EndPoint}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            Debug.Log($"[Server] Player disconnected: {disconnectInfo.Reason}");

            if (peer.Tag != null) {
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

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint,
                                                NetPacketReader reader,
                                                UnconnectedMessageType messageType) {
            // do nothing
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

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, SendType sendType) {
            byte packetType = reader.GetByte();
            if (packetType >= NetworkGeneral.PacketTypesCount)
                return;
            PacketType pt = (PacketType) packetType;
            switch (pt) {
                case PacketType.Movement: // 매번 클라에서 보내주고있음
                    OnInputReceived(reader, peer);
                    break;
                case PacketType.Serialized:
                    _packetProcessor.ReadAllPackets(reader, peer);
                    break;
                default:
                    Debug.Log($"[Server] OnNetworkReceive - Unhandled packet : {pt}");
                    break;
            }
        }
    }
}