using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Models
{
    public class Member
    {
        public Guid ConnectionInfoId { get; set; } // salvam id-ul ptu conexiune
        public string Color {  get; set; } // atribuirea unei culori a utilizatorului
        public string Name { get; set; } // atribuirea numelui a utilizatorului
    }
}
