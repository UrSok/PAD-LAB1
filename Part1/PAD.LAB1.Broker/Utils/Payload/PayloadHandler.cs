using PAD.LAB1.Broker.Models;
using PAD.LAB1.Broker.Storage;
using PAD.LAB1.Shared;
using PAD.LAB1.Shared.Models;
using PAD.LAB1.Shared.Models.Payload;
using System;

namespace PAD.LAB1.Broker.Utils
{
    public static class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes, Guid connectionInfoId)
        {
            var receivedPayload = Payload.GetPayloadFromBytes(payloadBytes); // convertam bitii primiti in payload
            var payloadRoute = new PayloadRoute(); // cream un payloadroute

            switch (receivedPayload.PayloadCommand)
            {
                case PayloadCommand.NewRoom: 
                    {
                        var room = RoomStorage.GenerateNewRoom(); // genereaza un room nou
                        room.AddMember(connectionInfoId, receivedPayload.Member.Name); // adaugam un membru nou
                        var newMember = room.GetMember(connectionInfoId); // extragem membrul nou primit
                        
                        payloadRoute.Payload = PayloadFactory.GetPayloadForWelcomeToRoomForPublisher(room, newMember);
                        payloadRoute.AddReceiver(connectionInfoId); // adaugam conexiunea cui o sa trimitem mesajele

                        BrokerUIStorage.EnqueueNewRoom(connectionInfoId, receivedPayload.Member.Name, room.Code);
                    }
                    break;
                case PayloadCommand.EnterRoom:
                    {
                        var room = RoomStorage.GetRoom(receivedPayload.Room.Code); // extragem room-ul de care avem nevoie

                        if (room == null) // in caz ca nu exista primim null
                        {
                            payloadRoute.Payload = PayloadFactory.GetPayloadForNoSuchRoom(receivedPayload.Room.Code);
                            payloadRoute.AddReceiver(connectionInfoId);

                            BrokerUIStorage.EnqueueNoSuchRoom(connectionInfoId, receivedPayload.Member.Name, receivedPayload.Room.Code);
                        }
                        else
                        {
                            room.AddMember(connectionInfoId, receivedPayload.Member.Name); // adaugam membrul in camera noua
                            var newMember = room.GetMember(connectionInfoId); // extragem membrul adaugat ultimul

                            // payload to be sent back to publisher
                            var payloadRouteForPublisher = new PayloadRoute();
                            payloadRouteForPublisher.Payload = PayloadFactory.GetPayloadForWelcomeToRoomForPublisher(room, newMember);
                            payloadRouteForPublisher.AddReceiver(connectionInfoId);
                            PayloadRouteStorage.Add(payloadRouteForPublisher);

                            // payload to be sent to subscribers
                            payloadRoute.Payload = PayloadFactory.GetPayloadForWelcomeToRoomForSubscribers(room, newMember);
                            payloadRoute.AddReceivers(room.GetAllMembersConnectionInfoIdsExceptPublisher(connectionInfoId));

                            BrokerUIStorage.EnqueueWelcomeToRoom(connectionInfoId, receivedPayload.Member.Name, room);
                        }
                    }
                    break;
                case PayloadCommand.SendMessage:
                    {
                        var room = RoomStorage.GetRoom(receivedPayload.Room.Code);
                        var member = room.GetMember(connectionInfoId);

                        // payload to be sent back to publisher
                        var payloadRouteForPublisher = new PayloadRoute();
                        payloadRouteForPublisher.Payload = PayloadFactory.GetPayloadForSendMessagePublisher(member, receivedPayload.Message);
                        payloadRouteForPublisher.AddReceiver(connectionInfoId);
                        PayloadRouteStorage.Add(payloadRouteForPublisher);

                        // payload to be sent to subscribers
                        payloadRoute.Payload = PayloadFactory.GetPayloadForSendMessageSubscribers(member, receivedPayload.Message);
                        payloadRoute.AddReceivers(room.GetAllMembersConnectionInfoIdsExceptPublisher(connectionInfoId));
                    }
                    break;
                default:
                    BrokerUIStorage.EnqueError(new Exception("Unknown Command"));
                    return;
            }

            PayloadRouteStorage.Add(payloadRoute);
        }

        public static void HandleMemberLostConnection(Guid connectionInfoId)
        {
            var payloadRoute = new PayloadRoute();

            var room = RoomStorage.GetRoomByMemberConnectionInfoId(connectionInfoId);
            var member = room?.GetMember(connectionInfoId); // room? -> daca room e null, atunci se va returna null si la member
            if (member == null)
            {
                BrokerUIStorage.EnqueueMemberWithNoRoomLostConnection(connectionInfoId);
                return;
            }

            room.RemoveMember(connectionInfoId);

            payloadRoute.Payload = PayloadFactory.GetPayloadForMemberLeft(room, member);

            var receivers = room.GetAllMembersConnectionInfoIdsExceptPublisher(connectionInfoId);
            payloadRoute.AddReceivers(receivers);

            BrokerUIStorage.EnqueueMemberLostConnection(member, room);

            PayloadRouteStorage.Add(payloadRoute);
        }
    }
}
