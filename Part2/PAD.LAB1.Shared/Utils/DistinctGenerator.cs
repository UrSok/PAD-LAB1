using System;
using System.Collections.Generic;
using System.Linq;

namespace PAD.LAB1.Shared.Utils
{
    public static class DistinctGenerator
    {
        public static string GenerateHexColor(IEnumerable<string> unavailableColors)
        {
            var random = new Random();
            var color = "";

            do
            {
                color = string.Format("#{0:X6}", random.Next(0x1000000));
            } while (unavailableColors.Any(x => x == color));

            return color;
        }

        public static string GenerateRoomCode(IEnumerable<string> unavailableRoomCodes)
        {
            var roomCode = "";

            do
            {
                roomCode = GenerateRoomChars();
            } while (unavailableRoomCodes.Any(x => x == roomCode));

            return roomCode;
        }

        private static string GenerateRoomChars()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var room = "";
            var roomLength = 4;
            var random = new Random();

            for (int i = 0; i < roomLength; i++)
            {
                room += chars[random.Next(chars.Length)];
            }

            return room;
        }
    }
}
