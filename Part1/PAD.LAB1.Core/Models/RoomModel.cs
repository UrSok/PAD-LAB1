using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Models
{
    public class RoomModel : MvxNotifyPropertyChanged
    {
        public string Code { get; set; }

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
