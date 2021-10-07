using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Models
{
    public enum UICommand
    {
        Unknown = 0,
        NewConnection = 1,
        NewRoom = 2,
        NoSuchRoom = 3,
        WelcomeToRoom = 4,
        MemberLeft = 5,
        MemberLostConnection = 6,
        Error = 7,
    }

    public class BrokerUIModel
    {
        public UICommand UICommand { get; set; }
        public Guid ConnectionInfoId { get; set; }
        public string RoomCode { get; set; }
        public int MembersCount { get; set; }
        public string MemberName {  get; set; }
        public Exception Exception { get; set; }
    }
}
