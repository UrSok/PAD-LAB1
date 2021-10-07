using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PAD.LAB1.Core.Models
{
    public class MessageModel
    {
        public string MemberName { get; set; }
        public string MemberColor { get; set; }
        public DateTime MessageDate { get; set; }
        public string MessageText { get; set; }
    }
}
