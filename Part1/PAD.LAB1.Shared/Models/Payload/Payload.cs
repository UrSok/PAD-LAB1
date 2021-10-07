using Newtonsoft.Json;
using PAD.LAB1.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Shared.Models.Payload
{
    public enum PayloadCommand
    {
        Unknown = 0, 
        NewRoom = 1, // client -> broker
        EnterRoom = 2, // client -> broker
        WelcomeToRoom = 3, // broker -> client
        NewMember = 4, // broker -> client
        NoSuchRoom = 5, // broker -> client
        MemberLeft = 6, // broker -> client
        NewMessage = 7, // broker -> client
        SendMessage = 8, // client -> broker
        MessageSent = 9, // broker -> client
    }

    public class Payload // cu ajutorul lui transmitem mesajele de care avem nevoie
    {
        public PayloadCommand PayloadCommand { get; set; }
        public PayloadRoom Room { get; set; }
        public PayloadMember Member { get; set; }
        public PayloadMessage Message { get; set; }

        public Payload() // cand se creaza payload-ul, el automat atribuie valoarea unknown
        {
            PayloadCommand = PayloadCommand.Unknown;
        }

        public static Payload GetPayloadFromBytes(byte[] payloadBytes) //convertim din biti in payload
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            return JsonConvert.DeserializeObject<Payload>(payloadString); //returnam obiectul primit
        }

        public byte[] GetBytes()
        {
            var payloadString = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(payloadString);//returnam bitii
        }
    }
}
