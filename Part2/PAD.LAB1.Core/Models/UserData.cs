using MvvmCross.ViewModels;
using PAD.LAB1.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Models
{
    public class UserData : MvxNotifyPropertyChanged
    {
        public string MemberId { get; set; }
        public string MemberColor { get; set; }
        public string MemberName { get; set; }
        public string RoomCode { get; set; }

        private int membersCount;
        public int MembersCount
        {
            get => membersCount;
            set
            {
                SetProperty(ref membersCount, value); 
                RaisePropertyChanged(nameof(MembersCount));
            }
        }

        public List<MessageInfoModel> MessageInfosHistory { get; }

        public UserData()
        {
            MessageInfosHistory = new List<MessageInfoModel>();
        }
    }
}
