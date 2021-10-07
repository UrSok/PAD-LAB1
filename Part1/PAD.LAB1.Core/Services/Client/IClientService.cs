using PAD.LAB1.Core.Models;
using PAD.LAB1.Shared.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Services.Client
{
    public interface IClientService
    {
        bool IsConnected { get; }
        void Connect(string BrokerIp, string port);
        void CreateRoom(string name);
        void EnterRoom(string name, string roomCode);
        void SendMessage(MessageModel message, string roomCode);
    }
}
