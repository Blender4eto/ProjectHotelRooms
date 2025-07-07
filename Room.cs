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

        public Room(int RoomNumber, string Type, int Capacity, decimal PricePerNight, bool Occupied, string GuestName)
        {
            this.RoomNumber = RoomNumber;
            this.Type = Type;
            this.Capacity = Capacity;
            this.PricePerNight = PricePerNight;
            this.Occupied = Occupied;
            this.GuestName = GuestName;
        }
    }
}
