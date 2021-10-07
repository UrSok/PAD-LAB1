using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Services.Broker
{
    public interface IBrokerService
    {
        bool Start(string brokerIp, string brokerPort);
    }
}
