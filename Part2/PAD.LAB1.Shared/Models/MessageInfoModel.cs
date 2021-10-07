namespace PAD.LAB1.Shared.Models
{
    public class MessageInfoModel
    {
        public MemberModel Member { get; set; } // Sender
        public MessageModel Message { get; set; }

        public MessageInfoModel()
        {

        }

        public MessageInfoModel(MemberModel sender, MessageModel message)
        {
            Member = sender;
            Message = message;
        }
    }
}
