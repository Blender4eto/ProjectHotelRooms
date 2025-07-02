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
                }
                Console.WriteLine(new string('-', 30));
            }
        }

    }
}
