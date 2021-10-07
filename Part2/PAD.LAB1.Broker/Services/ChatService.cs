namespace PAD.LAB1.Broker.Services
{
    using AutoMapper;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using GrpcAgent;
    using PAD.LAB1.Broker.Storages;
    using PAD.LAB1.Broker.Utils;
    using PAD.LAB1.Shared.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ChatService : Chat.ChatBase
    {
        private readonly IRootStorage storage; 
        private readonly IMapper mapper; 

        public ChatService(IRootStorage storage, IMapper mapper)
        {
            this.storage = storage;// storage - baza de date
            this.mapper = mapper;// permite ca dintr-un obiect sa faci alt obiect in mod automat
        }
// putem face override la metodele din proto
        public override Task<CreateRoomReply> CreateRoom(CreateRoomRequest request, ServerCallContext context)
        {
            var reply = new CreateRoomReply();

            if (string.IsNullOrEmpty(request.MemberName)) 
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "Name can't be empty"
                };

                return Task.FromResult(reply);
            }

            var room = storage.Rooms.NewRoom();
            var member = room.AddMember(request.MemberName);

            reply.ReplyInfo = new ReplyInfo
            {
                Status = ReplyStatus.Success,
            };
            reply.Room = mapper.Map<Room>(room); // RoomModel -> Room
            reply.Member = mapper.Map<Member>(member); // MemberModel -> Member

            var messageInfo = MessageInfoModelFactory.GetCreateJoinRoomMessageInfo(room.Code, member.Name);
            member.AddToMessageQueue(messageInfo);

            var createdMessageInfo = MessageInfoModelFactory.GetCreateRoomMessageInfo(member.Name);
            room.MessagesHistory.Add(createdMessageInfo);

            return Task.FromResult(reply);
        }

        public override Task<JoinRoomReply> JoinRoom(JoinRoomRequest request, ServerCallContext context)
        {
            var reply = new JoinRoomReply();

            if (string.IsNullOrEmpty(request.MemberName) || string.IsNullOrEmpty(request.RoomCode))
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "Both Member Name and Room Code must not be empty"
                };

                return Task.FromResult(reply);
            }

            var room = storage.Rooms.GetRoomByRoomCode(request.RoomCode);

            if (room == null)
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "No such room"
                };

                return Task.FromResult(reply);
            }

            var member = room.AddMember(request.MemberName);

            reply.ReplyInfo = new ReplyInfo
            {
                Status = ReplyStatus.Success,
            };
            reply.Room = mapper.Map<Room>(room);
            reply.Member = mapper.Map<Member>(member);
            reply.MessagesInfo.AddRange(mapper.Map<IEnumerable<MessageInfo>>(room.MessagesHistory));

            var messageInfoForRequester = MessageInfoModelFactory.GetCreateJoinRoomMessageInfo(room.Code, member.Name);
            member.AddToMessageQueue(messageInfoForRequester);

            var messageInfo = MessageInfoModelFactory.GetJoinRoomMessageInfo(member.Name);
            room.AddMessageForAllExceptSender(member.Id, messageInfo);

            return Task.FromResult(reply);
        }

        public override Task<MessageReply> SendMessage(MessageRequest request, ServerCallContext context)
        {
            var reply = new MessageReply();

            if (string.IsNullOrEmpty(request.MemberId) || string.IsNullOrEmpty(request.RoomCode)
                || string.IsNullOrEmpty(request.MessageText))
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "MemberId, RoomCode and MessagesText must not be empty!"
                };

                return Task.FromResult(reply);
            }

            var room = storage.Rooms.GetRoomByRoomCode(request.RoomCode);

            if (room == null)
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "No such room",
                };

                return Task.FromResult(reply);
            }

            var member = room.GetMember(request.MemberId);

            if (member == null)
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "No such member",
                };

                return Task.FromResult(reply);
            }

            var messageInfo = MessageInfoModelFactory.GetMessageInfoWithText(member, request.MessageText);
            room.AddMessage(messageInfo);

            reply.ReplyInfo = new ReplyInfo
            {
                Status = ReplyStatus.Success,
            };

            return Task.FromResult(reply);
        }

        public override async Task RoomStream(StreamRequest request, IServerStreamWriter<RoomStreamReply> responseStream, ServerCallContext context)
        {
            var room = storage.Rooms.GetRoomByRoomCode(request.RoomCode);

            var reply = new RoomStreamReply();
            if (room == null)
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "No such room",
                };
                await responseStream.WriteAsync(reply);
                return;
            }

            var membersCount = 0; // salvam local numarul de membri - cati sunt la moment in room; by default - 0
            do // rulam infinit
            {
                if (context.CancellationToken.IsCancellationRequested) // cu ea verificam daca clientul nu sa deconectat
                {
                    var member = room.GetMember(request.MemberId);// se extrage member id
                    var messageInfo = MessageInfoModelFactory.GetLeftRoomMessageInfo(member.Name);
                    room.RemoveMember(member.Id); // se sterge din camera 
                    room.AddMessage(messageInfo);
                    return; 
                }

                if (membersCount != room.MembersCount) // daca membersCount local difera de membersCount in room -> se transmite mesaj catre client
                {
                    reply.ReplyInfo = new ReplyInfo
                    {
                        Status = ReplyStatus.Success,
                    };
                    reply.MembersCount = room.MembersCount;

                    membersCount = room.MembersCount;
                    await responseStream.WriteAsync(reply);
                }
            } while (true);
        }

        public override async Task ChatStream(StreamRequest request, IServerStreamWriter<ChatStreamReply> responseStream, ServerCallContext context)
        {
            var room = storage.Rooms.GetRoomByRoomCode(request.RoomCode);

            var reply = new ChatStreamReply();
            if (room == null)
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "No such room",
                };
                await responseStream.WriteAsync(reply);
                return;
            }

            var member = room.GetMember(request.MemberId);

            if (member == null)
            {
                reply.ReplyInfo = new ReplyInfo
                {
                    Status = ReplyStatus.Failed,
                    Reason = "No such member",
                };
                await responseStream.WriteAsync(reply);
                return;
            }

            do
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                while (!member.IsMessageQueueEmpty)
                {
                    var messageInfo = member.GetNextMessage();

                    if (messageInfo == null) continue;

                    reply.ReplyInfo = new ReplyInfo
                    {
                        Status = ReplyStatus.Success,
                    };
                    reply.MessageInfo = new MessageInfo();
                    reply.MessageInfo.Member = mapper.Map<Member>(messageInfo.Member);
                    reply.MessageInfo.Message = mapper.Map<Message>(messageInfo.Message);
                    await responseStream.WriteAsync(reply);
                }
            } while (true);
        }
    }
}
