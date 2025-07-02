using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectHotelRooms
{
    using System.Text.Json;
    using static Constants;
    public  class Data
    {
        public List<Room> Rooms { get; private set; }

        private StreamReader reader;
        private StreamWriter writer;

        public Data()
        {
            LoadRooms();
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter(filePath);
            using (writer)
            {
                string jsonData = JsonSerializer.Serialize(Rooms);
                writer.Write(jsonData);
            }
        }

        public void LoadRooms()
        {
            reader = new StreamReader(filePath);
            using (reader)
            {
                string jsonData = reader.ReadToEnd();
                Rooms = JsonSerializer.Deserialize<List<Room>>(jsonData)!;
            }
            Rooms ??= new List<Room>();
        }

        public void DisplayRooms()
        {
            foreach (var room in Rooms)
            {

                Console.WriteLine($"Room {room.RoomNumber} - {room.Type}");
                Console.WriteLine($"  Capacity: {room.Capacity}");
                Console.WriteLine($"  Price per night: {room.PricePerNight:C}");
                Console.WriteLine($"  Occupied: {(room.Occupied ? "Yes" : "No")}");
                if (room.Occupied)
                {
                    Console.WriteLine($"  Guest Name: {room.GuestName}");
                    Console.WriteLine("");
                }
                else { Console.WriteLine(""); }
                   
            }
        }

        public void ReservateRoom()
        {
            List<Room> availableRooms = new List<Room>();

            foreach (var room in Rooms)
            {
                if (!room.Occupied)
                {
                    availableRooms.Add(room);
                }
            }

            if (availableRooms.Count == 0)
            {
                Console.WriteLine("Sorry, no available rooms to reserve.");
                return;
            }

            Console.WriteLine("Available rooms:");
            foreach (var room in availableRooms)
            {
                Console.WriteLine($"Room {room.RoomNumber} - {room.Type} - Capacity: {room.Capacity} - Price: {room.PricePerNight:C}");
            }

            Console.Write("Enter the room number you want to reserve: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int selectedRoomNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid room number.");
                return;
            }

            Room roomToReserve = null;
            foreach (var room in availableRooms)
            {
                if (room.RoomNumber == selectedRoomNumber)
                {
                    roomToReserve = room;
                    break;
                }
            }

            Console.Write("Enter your name: ");
            string guestName = Console.ReadLine();

            roomToReserve.Occupied = true;
            roomToReserve.GuestName = guestName;
            Save();
        }

        

        


    }
}
