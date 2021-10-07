using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Shared.Models.UI
{
    public enum ClientUICommand
    {
        Unknown = 0,
        NoSuchRoom = 1,
        WelcomeToRoom = 2,
        NewMember = 3,
        NewMessage = 4,
        MemberLeft = 5,
    }

    public class ClientUIModel
    {
        public ClientUICommand Command { get; set; }
        public RoomUIModel Room { get; set; }
        public MemberUIModel Member { get; set; }
        public MessageUIModel Message {  get; set; }
    }
}
