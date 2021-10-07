using PAD.LAB1.Broker.Models;
using PAD.LAB1.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Storage
{
// ca la Ernest
    public static class PayloadRouteStorage
    {
        private static readonly ConcurrentQueue<PayloadRoute> payloadRouteQueue;

        static PayloadRouteStorage()
        {
            payloadRouteQueue = new ConcurrentQueue<PayloadRoute>();
        }

        public static void Add(PayloadRoute payloadRoute) // ptu a adauga
        {
            payloadRouteQueue.Enqueue(payloadRoute);
        }

        public static PayloadRoute GetNext() // ptu a extrage urmatorul
        {
            payloadRouteQueue.TryDequeue(out PayloadRoute payloadRoute);
            return payloadRoute;
        }

        public static bool IsEmpty => payloadRouteQueue.IsEmpty;

        public static int Count => payloadRouteQueue.Count;
    }
}
