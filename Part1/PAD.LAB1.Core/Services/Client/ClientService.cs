using PAD.LAB1.Client;
using PAD.LAB1.Client.Storage;
using PAD.LAB1.Client.Utils;
using PAD.LAB1.Core.Models;
using PAD.LAB1.Shared;
using PAD.LAB1.Shared.Models.Payload;
using PAD.LAB1.Shared.Models.UI;
using PAD.LAB1.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Services.Client
{
    public class ClientService : IClientService
    {
        private readonly ClientNetworkMananger clientNetworkMananger;

        public bool IsConnected => clientNetworkMananger.IsConnected;

        public ClientService()
        {
            clientNetworkMananger = new ClientNetworkMananger();
        }

        public void Connect(string brokerIp, string brokerPort)
        {
            GenericValidator.CheckIfEmptyString(brokerIp, nameof(brokerIp));
            GenericValidator.CheckIfEmptyString(brokerPort, nameof(brokerPort));

            clientNetworkMananger.Connect(brokerIp, Convert.ToInt32(brokerPort));
        }

        public void CreateRoom(string name)
        {
            GenericValidator.CheckIfEmptyString(name, nameof(name));

            var payload = PayloadFactory.GetPayloadForNewRoom(name);
            clientNetworkMananger.Send(payload);
        }

        public void EnterRoom(string name, string roomCode)
        {
            GenericValidator.CheckIfEmptyString(name, nameof(name));
            GenericValidator.CheckIfEmptyString(roomCode, nameof(roomCode));

            var payload = PayloadFactory.GetPayloadForEnterRoom(name, roomCode);
            clientNetworkMananger.Send(payload);
        }

        public void SendMessage(MessageModel message, string roomCode)
        {
            var payload = PayloadFactory.GetPayloadForSendMessage(roomCode, message.MemberColor, message.MemberName, message.MessageDate, message.MessageText);

            clientNetworkMananger.Send(payload);
        }
    }
}
