using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Models
{
    public class LocalUserData : MvxNotifyPropertyChanged
    {
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
    }
}
