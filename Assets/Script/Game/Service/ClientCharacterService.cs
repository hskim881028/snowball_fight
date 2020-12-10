// using SF.Common.Packet;
// using SF.Common.Packet.ManualSerializable;
// using SF.Common.Util;
// using UnityEngine;
//
// namespace SF.Service {
//     public class ClientCharacterService {
//         private readonly Client _client;
//         private readonly CircularBuffer<MovePacket> _predictionMovePackets;
//         
//         private ServerStatePacket _lastServerStatePacket;
//         
//         private const int MaxStoredPackets = 60;
//         private bool _isFirstStateReceived;
//
//         private Character.Character _lastCharacter;
//
//         public ClientCharacterService(Client client) {
//             _client = client;
//             _predictionMovePackets = new CircularBuffer<MovePacket>(MaxStoredPackets);
//         }
//
//         public void ReceiveServerStatePacket(ServerStatePacket serverStatePacket, CharacterPacket characterPacket) // 서버에서 정보를 받았을떄 들어옴
//         {
//             if (!_isFirstStateReceived) {
//                 if (serverStatePacket.LastProcessedAction == 0) {
//                     return;
//                 }
//                     
//                 _isFirstStateReceived = true;
//             }
//
//             if (serverStatePacket.Tick == _lastServerStatePacket.Tick || serverStatePacket.LastProcessedAction ==
//                 _lastServerStatePacket.LastProcessedAction) {
//                 return;
//             }
//                 
//
//             _lastServerStatePacket = serverStatePacket;
//
//             //sync
//             _position = characterPacket.Position;
//             _rotation = characterPacket.Rotation;
//
//             // todo : mono로 업데이트 돌지말고 여기서 view 업데이트 쳐줘야함
//             if (_predictionPlayerInputPackets.Count == 0)
//                 return;
//
//             ushort lastProcessedCommand = serverStatePacket.LastProcessedCommand;
//             int diff = NetworkGeneral.SeqDiff(lastProcessedCommand, _predictionPlayerInputPackets.First.Id);
//
//             //apply prediction
//             if (diff >= 0 && diff < _predictionPlayerInputPackets.Count) {
//                 //Debug.Log($"[OK]  SP: {serverState.LastProcessedCommand}, OUR: {_predictionPlayerStates.First.Id}, DF:{diff}");
//                 _predictionPlayerInputPackets.RemoveFromStart(diff + 1);
//                 foreach (var state in _predictionPlayerInputPackets)
//                     ApplyInput(state, LogicTimer.FixedDelta);
//             }
//             else if (diff >= _predictionPlayerInputPackets.Count) {
//                 Debug.Log(
//                     $"[C] Player input lag st: {_predictionPlayerInputPackets.First.Id} ls:{lastProcessedCommand} df:{diff}");
//                 //lag
//                 _predictionPlayerInputPackets.FastClear();
//                 _nextCommand.Id = lastProcessedCommand;
//             }
//             else {
//                 Debug.Log(
//                     $"[ERR] SP: {serverStatePacket.LastProcessedCommand}, OUR: {_predictionPlayerInputPackets.First.Id}, DF:{diff}, STORED: {StoredCommands}");
//             }
//         }
//
//         public void Movement() { // 여기 클라이언트 로직업데이트에서 불림 즉 현재 갱신직전 마지막정보 캐싱해주고 센드해줘야됨
//             _client.SendPacketSerializable(PacketType.Movement, ToString(),);
//             _updateCount++;
//             if (_updateCount == 3) {
//                 _updateCount = 0;
//                 foreach (var t in _predictionPlayerStates) {
//                     Debug.Log($"[PacketService] Movement : {t.Id}");
//                     _client.SendPacketSerializable(PacketType.Movement, t, DeliveryMethod.Unreliable);
//                 }
//             }
//         }
//     }
// }