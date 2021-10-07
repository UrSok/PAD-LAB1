using PAD.LAB1.Shared.Models;
using PAD.LAB1.Shared.Utils;
using System.Collections.Concurrent;

namespace PAD.LAB1.Broker.Models
{
    public class ServerMemberModel : MemberModel
    {
        private readonly ConcurrentQueue<MessageInfoModel> messages;

        public ServerMemberModel(string name, string hexColor) : base(name, hexColor)
        {
            messages = new ConcurrentQueue<MessageInfoModel>();
        }

        public void AddToMessageQueue(MessageInfoModel messageInfo)
        {
            messages.Enqueue(messageInfo);
        }

        public MessageInfoModel GetNextMessage()
        {
            messages.TryDequeue(out var messageInfo);
            return messageInfo;
        }

        public bool IsMessageQueueEmpty => messages.IsEmpty;

    }
}
