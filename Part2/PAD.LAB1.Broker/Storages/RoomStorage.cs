using PAD.LAB1.Broker.Models;
using PAD.LAB1.Shared.Models;
using PAD.LAB1.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD.LAB1.Broker.Storages
{
// se salveaza room-urile care le vom utiliza
    public interface IRoomStorage
    {
        RoomModel NewRoom();
        RoomModel GetRoomByRoomCode(string roomCode);
        RoomModel GetRoomByMemberId(string memberId);
    }

    public class RoomStorage : IRoomStorage
    {
        public readonly List<RoomModel> rooms;

        public RoomStorage()
        {
            rooms = new List<RoomModel>();
        }

        public RoomModel NewRoom()
        {
            var room = new RoomModel
            {// se creaza codul ptu camera
            // DistinctGenerator ptu generarea distincta in dependenta de ce transmitem
                Code = DistinctGenerator.GenerateRoomCode(rooms.Select(x => x.Code)),
            };

            rooms.Add(room);
            return room;
        }

        public RoomModel GetRoomByMemberId(string memberId)
        {
            return rooms.FirstOrDefault(x => x.IsMemberInRoom(memberId));
        }

        public RoomModel GetRoomByRoomCode(string roomCode)
        {
            return rooms.FirstOrDefault(x => x.Code == roomCode);
        }
    }
}
