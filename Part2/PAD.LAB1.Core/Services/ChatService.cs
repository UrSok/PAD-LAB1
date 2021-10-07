using Grpc.Net.Client;
using GrpcAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Services
{
    public interface IChatService
    {
        GrpcChannel Channel { get; }
        Chat.ChatClient Client { get; }
    }

    public class ChatService : IChatService
    {
        public GrpcChannel Channel { get; }
        public Chat.ChatClient Client { get; }
        
        public ChatService()
        {
            Channel = GrpcChannel.ForAddress("https://localhost:5001");
            Client = new Chat.ChatClient(Channel);
        }
    }
}
