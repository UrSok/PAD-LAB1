using PAD.LAB1.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Services.Broker
{
    public class BrokerService : IBrokerService
    {
        private readonly BrokerNetworkManager brokerNetworkMananger;

        public BrokerService()
        {
            brokerNetworkMananger = new BrokerNetworkManager();
        }

        public bool Start(string brokerIp, string brokerPort)
        {
            try
            {
                brokerNetworkMananger.Start(brokerIp, Convert.ToInt32(brokerPort));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
