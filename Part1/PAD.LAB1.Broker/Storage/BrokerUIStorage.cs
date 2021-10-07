using PAD.LAB1.Broker.Models;
using PAD.LAB1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Storage
{
// salvam comenzile ptu UI ca sa afisam ceva
// avem mai multe metode, in dependenta de ce metoda folosim - avem diferite actiuni care se vor face
    public static class BrokerUIStorage
    {
        private static readonly Queue<BrokerUIModel> brokerUIModelQueue;

        static BrokerUIStorage()
        {
            brokerUIModelQueue = new Queue<BrokerUIModel>();
        }
        public static void EnqueNewConnection(Guid connectionInfoId)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.NewConnection,
                ConnectionInfoId = connectionInfoId
            });
        }

        public static void EnqueError(Exception exception)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.Error,
                Exception = exception
            });
        }

        public static void EnqueueNewRoom(Guid connectionInfoId, string memeberName, string roomCode)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.NewRoom,
                ConnectionInfoId = connectionInfoId,
                RoomCode = roomCode,
                MemberName = memeberName,
                MembersCount = 1,
            });
        }

        public static void EnqueueNoSuchRoom(Guid connectionInfoId, string memeberName, string roomCode)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.NoSuchRoom,
                ConnectionInfoId = connectionInfoId,
                RoomCode = roomCode,
                MemberName = memeberName
            });
        }

        public static void EnqueueWelcomeToRoom(Guid connectionInfoId, string memeberName, Room room)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.WelcomeToRoom,
                ConnectionInfoId = connectionInfoId,
                RoomCode = room.Code,
                MemberName = memeberName,
                MembersCount = room.MembersCount
            });
        }

        public static void EnqueueMemberLeft(Member member, Room room)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.MemberLeft,
                ConnectionInfoId = member.ConnectionInfoId,
                RoomCode = room.Code,
                MemberName = member.Name,
                MembersCount = room.MembersCount
            });
        }

        public static void EnqueueMemberLostConnection(Member member, Room room)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.MemberLostConnection,
                ConnectionInfoId = member.ConnectionInfoId,
                RoomCode = room.Code,
                MemberName = member.Name,
                MembersCount = room.MembersCount
            });
        }

        public static void EnqueueMemberWithNoRoomLostConnection(Guid connectionInfoId)
        {
            brokerUIModelQueue.Enqueue(new BrokerUIModel
            {
                UICommand = UICommand.MemberLostConnection,
                ConnectionInfoId = connectionInfoId
            });
        }

        public static BrokerUIModel GetNext()
        {
            return !IsEmpty ? brokerUIModelQueue.Dequeue() : null;
        }

        public static bool IsEmpty => brokerUIModelQueue.Count == 0;


    }
}
