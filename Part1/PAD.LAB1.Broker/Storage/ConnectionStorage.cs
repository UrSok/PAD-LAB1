using PAD.LAB1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Storage
{
// ca la Ernest in video
    public static class ConnectionStorage
    {
        private static readonly List<ConnectionInfo> connectionInfos; // lista cu conexiuni curenta
        private static readonly object locker;  

        static ConnectionStorage()
        {
            connectionInfos = new List<ConnectionInfo>();
            locker = new object();
        }

        public static void Add(ConnectionInfo connectionInfo)
        {
            lock (locker)
            {
                connectionInfos.Add(connectionInfo);
            }
        }

        public static void Remove(Guid connectionInfoId)
        {
            lock (locker)
            {
                connectionInfos.RemoveAll(x => x.Id == connectionInfoId);
            }
        }

        public static List<ConnectionInfo> GetReceivers(List<Guid> connectionInfoIds)
        {
            // daca x este in connectionInfoIds - atunci il adaugam in lista
            return connectionInfos.Where(x => connectionInfoIds.Contains(x.Id)).ToList(); // extragem userii care vor primi mesajul
        }

        public static int ConnectionsCount => connectionInfos.Count;
    }
}
