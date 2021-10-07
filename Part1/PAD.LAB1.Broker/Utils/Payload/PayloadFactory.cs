using PAD.LAB1.Broker.Models;
using PAD.LAB1.Shared.Models.Payload;

namespace PAD.LAB1.Broker.Utils
{
    public static class PayloadFactory
    {
        //metode statice care returneaza payload-ul de care avem nevoie
        public static Payload GetPayloadForNoSuchRoom(string roomCode)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.NoSuchRoom,
                Room = new PayloadRoom
                {
                    Code = roomCode,
                }
            };
        }

        public static Payload GetPayloadForWelcomeToRoomForPublisher(Room room, Member member)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.WelcomeToRoom,
                Room = new PayloadRoom
                {
                    Code = room.Code,
                    MembersCount = room.MembersCount,
                },
                Member = new PayloadMember
                {
                    Color = member.Color,
                    Name = member.Name,
                }
            };
        }

        public static Payload GetPayloadForWelcomeToRoomForSubscribers(Room room, Member member)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.NewMember,
                Room = new PayloadRoom
                {
                    Code = room.Code,
                    MembersCount = room.MembersCount,
                },
                Member = new PayloadMember
                {
                    Color = member.Color,
                    Name = member.Name,
                }
            };
        }

        public static Payload GetPayloadForSendMessageSubscribers(Member member, PayloadMessage message)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.NewMessage,
                Member = new PayloadMember
                {
                    Color = member.Color,
                    Name = member.Name
                },
                Message = new PayloadMessage
                {
                    DateTime = message.DateTime,
                    Text = message.Text
                }
            };
        }

        public static Payload GetPayloadForSendMessagePublisher(Member member, PayloadMessage message)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.MessageSent,
                Member = new PayloadMember
                {
                    Color = member.Color,
                    Name = member.Name
                },
                Message = new PayloadMessage
                {
                    DateTime = message.DateTime,
                    Text = message.Text
                }
            };
        }

        public static Payload GetPayloadForMemberLeft(Room room, Member member)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.MemberLeft,
                Room = new PayloadRoom
                {
                    MembersCount = room.MembersCount,
                },
                Member = new PayloadMember
                {
                    Color = member.Color,
                    Name = member.Name
                },
            };
        }
    }
}
