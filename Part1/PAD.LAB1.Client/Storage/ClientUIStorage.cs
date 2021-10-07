using PAD.LAB1.Shared.Models;
using PAD.LAB1.Shared.Models.Payload;
using PAD.LAB1.Shared.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Client.Storage
{
    public static class ClientUIStorage
    {
        private static readonly Queue<ClientUIModel> clientUIModelQueue;

        static ClientUIStorage()
        {
            clientUIModelQueue = new Queue<ClientUIModel>();
        }

        public static void EnqueNoSuchRoom(string roomCode)
        {
            clientUIModelQueue.Enqueue(new ClientUIModel
            {
                Command = ClientUICommand.NoSuchRoom,
                Room = new RoomUIModel
                {
                    Code = roomCode,
                }
            });
        }

        public static void EnqueWelcomeToRoom(PayloadRoom room, PayloadMember member)
        {
            clientUIModelQueue.Enqueue(new ClientUIModel
            {
                Command = ClientUICommand.WelcomeToRoom,
                Room = new RoomUIModel
                {
                    Code = room.Code,
                    MembersCount = room.MembersCount
                },
                Member = new MemberUIModel
                {
                    Color = member.Color,
                    Name = member.Name,
                }
            });
        }

        public static void EnqueNewMember(int membersCount, PayloadMember member)
        {
            clientUIModelQueue.Enqueue(new ClientUIModel
            {
                Command = ClientUICommand.NewMember,
                Room = new RoomUIModel
                {
                    MembersCount = membersCount
                },
                Member = new MemberUIModel
                {
                    Color = member.Color,
                    Name = member.Name,
                }
            });
        }

        public static void EnqueMemberLeft(int membersCount, PayloadMember member)
        {
            clientUIModelQueue.Enqueue(new ClientUIModel
            {
                Command = ClientUICommand.MemberLeft,
                Room = new RoomUIModel
                {
                    MembersCount = membersCount
                },
                Member = new MemberUIModel
                {
                    Color = member.Color,
                    Name = member.Name,
                }
            });
        }

        public static void EnqueNewMessage(PayloadMember member, PayloadMessage message)
        {
            clientUIModelQueue.Enqueue(new ClientUIModel
            {
                Command = ClientUICommand.NewMessage,
                Member = new MemberUIModel
                {
                    Color = member.Color,
                    Name = member.Name,
                },
                Message = new MessageUIModel
                {
                    DateTime = message.DateTime,
                    Text = message.Text,
                }
            });
        }

        public static ClientUIModel GetNext()
        {
            return !IsEmpty ? clientUIModelQueue.Dequeue() : null;
        }

        public static bool IsEmpty => clientUIModelQueue.Count == 0;

    }
}
