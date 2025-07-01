using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHotelRooms
{
#nullable disable
    public class Room
    {
        public int RoomNumber { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public bool Occupied { get; set; }
        public string GuestName { get; set; }

        public Room(int roomNum, string type, int capacity, decimal pricePerNight, bool occupied, string guestName)
        {

            RoomNumber = roomNum;
            Type = type;
            Capacity = capacity;
            PricePerNight = pricePerNight;
            Occupied = occupied;
            GuestName = guestName;
        }

    }
}
