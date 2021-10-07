using PAD.LAB1.Broker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Broker.Storage
{
//se salveaza lista de camere disponibile
    public static class RoomStorage
    {
        private static readonly List<Room> rooms;

        static RoomStorage()
        {
            rooms = new List<Room>();
        }

        public static Room GenerateNewRoom()
        {
            var roomCode = "";

            do
            {
                roomCode = GenerateRoomChars();
            } while (rooms.Any(x => x.Code == roomCode));

            var room = new Room(roomCode);

            rooms.Add(room);
            return room;
        }

        private static string GenerateRoomChars()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var room = "";
            var roomLength = 4;
            var random = new Random();

            for (int i = 0; i < roomLength; i++)
            {
                room += chars[random.Next(chars.Length)];
            }

            return room;
        }

        public static Room GetRoom(string code)
        {
            return rooms.FirstOrDefault(x => x.Code == code);
        }
        public static Room GetRoomByMemberConnectionInfoId(Guid connectionInfoId) 
        {
            return rooms.FirstOrDefault(x => x.GetMember(connectionInfoId) != null);
        }

        public static int RoomsCount => rooms.Count();
    }
}
