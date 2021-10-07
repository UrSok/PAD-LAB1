using PAD.LAB1.Shared;
using PAD.LAB1.Shared.Models.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Models
{
//cu ajutorul ei intelegem ce si cui trebuie sa transmitem
    public class PayloadRoute
    {
        public Payload Payload { get; set; }
        private List<Guid> connectionInfoIds;

        public PayloadRoute()
        {
            connectionInfoIds = new List<Guid>();
        }

        public void AddReceiver(Guid connectionInfoId) // adaugarea unui singur receiver
        {
            connectionInfoIds.Add(connectionInfoId);
        }
        public void AddReceivers(List<Guid> connectionInfoIds) // sau adaugarea mai multor receiveri
        {
            this.connectionInfoIds.AddRange(connectionInfoIds);
        }

        public List<Guid> ConnectionInfoIds => connectionInfoIds;
    }
}
