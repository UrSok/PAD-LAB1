using PAD.LAB1.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Models
{
// chat-ul / camera unde trimit mesaje
    public class Room
    {
        public string Code { get; set; } // codul la room
        private readonly List<Member> members; // membrii room-ului

        public Room(string code)
        {
            Code = code;
            members = new List<Member>();
        }

        public void AddMember(Guid connectionInfoId, string name)
        {
            members.Add(new Member
            {
                ConnectionInfoId = connectionInfoId,
                Name = name,
                Color = GenerateDistinctHexColor()
            });
        }

        public void RemoveMember(Guid connectionInfoId)
        {
            members.RemoveAll(x => x.ConnectionInfoId == connectionInfoId);
        }

        private string GenerateDistinctHexColor() // metoda pentru generarea culorii random ptu utilizator
        {
            var random = new Random();
            var color = "";

            do
            {
                color = string.Format("#{0:X6}", random.Next(0x1000000));
            } while (members.Any(x => x.Color == color) && members.Count > 0);

            return color;
        }

        public int MembersCount => members.Count(); // returneaza numarul de membri

        public Member GetMember(Guid connectionInfoId) // returnam un membru
        {
            return members.FirstOrDefault(x => x.ConnectionInfoId == connectionInfoId);
        }
        public List<Guid> GetAllMembersConnectionInfoIdsExceptPublisher(Guid connectionInfoId) //returnam toate id-urile din connectionInfoId de pe membru, toti in afara de persoana care a trimis mesajul
        {
            // cu select, din Member extragem coloana InfoID si la urma avem o lista de ID-uri
            return members.Where(x => x.ConnectionInfoId != connectionInfoId).Select(x => x.ConnectionInfoId).ToList();
        }

    }
}
