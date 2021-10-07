using PAD.LAB1.Client.Storage;
using PAD.LAB1.Shared;
using PAD.LAB1.Shared.Models.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Client.Utils
{
    public static class PayloadHandler
    {
        // Received Payload Stack?
        public static void Handle(byte[] payloadBytes)
        {
            var payload = Payload.GetPayloadFromBytes(payloadBytes);// extragem payload-ul

            switch (payload.PayloadCommand) //evaluam comanda primita in payload
            {
                case PayloadCommand.NoSuchRoom:
                    ClientUIStorage.EnqueNoSuchRoom(payload.Room.Code);
                    break;
                case PayloadCommand.WelcomeToRoom:
                    ClientUIStorage.EnqueWelcomeToRoom(payload.Room, payload.Member);
                    break;
                case PayloadCommand.NewMember:
                    ClientUIStorage.EnqueNewMember(payload.Room.MembersCount, payload.Member);
                    break;
                case PayloadCommand.NewMessage:
                    ClientUIStorage.EnqueNewMessage(payload.Member, payload.Message);
                    break;
                case PayloadCommand.MessageSent:
                    ClientUIStorage.EnqueNewMessage(payload.Member, payload.Message);
                    break;
                case PayloadCommand.MemberLeft:
                    ClientUIStorage.EnqueMemberLeft(payload.Room.MembersCount, payload.Member);
                    break;
                default:
                    throw new Exception("Unknown command");
            }
        }
    }
}
