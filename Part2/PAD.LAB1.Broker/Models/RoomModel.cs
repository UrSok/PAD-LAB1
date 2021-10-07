using PAD.LAB1.Shared.Models;
using PAD.LAB1.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Models
{
    public class RoomModel
    {
        public string Code { get; set; }
        private readonly List<ServerMemberModel> members;
        public List<MessageInfoModel> MessagesHistory { get; }

        public RoomModel()
        {
            members = new List<ServerMemberModel>();
            MessagesHistory = new List<MessageInfoModel>();
        }

        public ServerMemberModel AddMember(string name)
        {
            var memberColor = DistinctGenerator.GenerateHexColor(members.Select(x => x.HexColor));
            var newMember = new ServerMemberModel(name, memberColor);

            members.Add(newMember);
            return newMember;
        }

        public void RemoveMember(string memberId)
        {
            members.RemoveAll(x => x.Id == memberId);
        }

        public ServerMemberModel GetMember(string memberId)
        {
            return members.FirstOrDefault(x => x.Id == memberId);
        }

        public bool IsMemberInRoom(string memberId)
        {
            return members.Any(x => x.Id == memberId);
        }

        public void AddMessageForAllExceptSender(string senderId, MessageInfoModel messageInfo)
        {
            MessagesHistory.Add(messageInfo);
            members.Where(x => x.Id != senderId).ToList()
                .ForEach(receiver => receiver.AddToMessageQueue(messageInfo));
        }

        public void AddMessage(MessageInfoModel messageInfo)
        {
            MessagesHistory.Add(messageInfo);
            members.ForEach(receiver => receiver.AddToMessageQueue(messageInfo));
        }

        public int MembersCount => members.Count;
    }
}
