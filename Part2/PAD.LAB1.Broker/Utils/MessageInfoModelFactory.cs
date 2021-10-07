using GrpcAgent;
using PAD.LAB1.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Utils
{
// cream mesaje in dependenta de metoda
    public static class MessageInfoModelFactory
    {
        public static MessageInfoModel GetCreateJoinRoomMessageInfo(string roomCode, string memberName)
        {
            return new MessageInfoModel(MemberModel.GetSystemMember(), new MessageModel
            {
                DateTime = DateTime.UtcNow,
                Text = $"Welcome { memberName }! \nUse this code to invite others: { roomCode }."
            });
        }

        public static MessageInfoModel GetMessageInfoWithText(MemberModel sender, string text)
        {
            return new MessageInfoModel(sender, new MessageModel
            {
                DateTime = DateTime.UtcNow,
                Text = text
            });
        }

        public static MessageInfoModel GetCreateRoomMessageInfo(string memberName)
        {
            return new MessageInfoModel(MemberModel.GetSystemMember(), new MessageModel
            {
                DateTime = DateTime.UtcNow,
                Text = $"{ memberName } created the room!"
            });
        }

        public static MessageInfoModel GetJoinRoomMessageInfo(string memberName)
        {
            return new MessageInfoModel(MemberModel.GetSystemMember(), new MessageModel
            {
                DateTime = DateTime.UtcNow,
                Text = $"{ memberName } joined the room!"
            });
        }

        public static MessageInfoModel GetLeftRoomMessageInfo(string memberName)
        {
            return new MessageInfoModel(MemberModel.GetSystemMember(), new MessageModel
            {
                DateTime = DateTime.UtcNow,
                Text = $"{ memberName } left the room!"
            });
        }
    }
}
