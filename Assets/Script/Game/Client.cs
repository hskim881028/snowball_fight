using System;
using System.Net;
using System.Net.Sockets;
using LibChaiiLatte;

namespace SF {
    public class Client : INetEventListener {
        public Client() {
            Random rand = new Random();
            
        }

        public void OnPeerConnected(NetPeer peer) {
            throw new System.NotImplementedException();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            throw new System.NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
            throw new System.NotImplementedException();
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, SendType sendType) {
            throw new System.NotImplementedException();
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
            throw new System.NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
            throw new System.NotImplementedException();
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            throw new System.NotImplementedException();
        }
    }
}