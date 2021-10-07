using PAD.LAB1.Shared;
using PAD.LAB1.Shared.Models.Payload;
using PAD.LAB1.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Client.Utils
{
    public static class PayloadFactory
    {
        public static Payload GetPayloadForNewRoom(string name)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.NewRoom,
                Member = new PayloadMember
                {
                    Name = name,
                }
            };
        }

        public static Payload GetPayloadForEnterRoom(string name, string roomCode)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.EnterRoom,
                Room = new PayloadRoom
                {
                    Code = roomCode,
                },
                Member = new PayloadMember
                {
                    Name = name,
                }
            };
        }

        public static Payload GetPayloadForSendMessage(string roomCode, string memberColor, string memberName, DateTime messageDate, string messageText)
        {
            return new Payload
            {
                PayloadCommand = PayloadCommand.SendMessage,
                Room = new PayloadRoom
                {
                    Code = roomCode,
                },
                Member = new PayloadMember
                {
                    Color = memberColor,
                    Name = memberName,
                },
                Message = new PayloadMessage
                {
                    DateTime = messageDate,
                    Text = messageText,
                }
            };
        }
    }
}
