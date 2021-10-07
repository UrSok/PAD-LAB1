using PAD.LAB1.Broker.Storage;
using PAD.LAB1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Utils
{
//clasa ce se ocupa de transmiterea datelor catre subcriber - in background
    public class PayloadRouter
    {
        private const int TimeToSleep = 500;

        public void Route()
        {
            while (true) // ruleaza infinit
            {
                while (!PayloadRouteStorage.IsEmpty) // cand se gaseste macar un obiect in PayloadRouteStorage, atunci se executa mai departe
                {
                    var payloadRoute = PayloadRouteStorage.GetNext(); // extragem urmatorul din lista
                    var payloadBytes = payloadRoute.Payload.GetBytes(); // convertim in biti

                    var receivers = ConnectionStorage.GetReceivers(payloadRoute.ConnectionInfoIds); // extragem receiverii din ConnectionStorage in baza la ConnectionInfoIds din payload route

                    foreach (var receiver in receivers) // un foreach prin toti receiverii
                    {
                        try
                        {
                            receiver.Socket.Send(payloadBytes); // transmitem bitii care iam facut din payload
                        }
                        catch
                        {
                            BrokerNetworkManager.MemberLostConnection(receiver);
                        }
                    }

                }

                Thread.Sleep(TimeToSleep);
            }
        }
    }
}
