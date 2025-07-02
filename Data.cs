using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectHotelRooms
{
    // TODO: Check if everything is correct, if not, fix it
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
                if (!string.IsNullOrEmpty(jsonData))
                {
                Rooms = JsonSerializer.Deserialize<List<Room>>(jsonData)!;
                }
            }
            Rooms ??= new List<Room>();
        }



        //------------------------------------------------------------------------
        public void DisplayRoomsAll()
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
        public void DisplayRoomsAvaliable()
        {
            foreach (var room in Rooms)
            {
                if (!room.Occupied)
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
        }

        public void DisplayRoomsUnavaliable()
        {
            foreach (var room in Rooms)
            {
                if (room.Occupied)
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

        public void ExitRoom()
        {
            List<Room> unavailableRooms = new List<Room>();

            foreach (var room in Rooms)
            {
                if (room.Occupied)
                {
                    unavailableRooms.Add(room);
                }
            }

            if (unavailableRooms.Count == 0)
            {
                Console.WriteLine("Sorry,all rooms are available rooms to reserve.");
                return;
            }

            foreach (var room in unavailableRooms)
            {
                Console.WriteLine($"Room {room.RoomNumber} - {room.Type} - Capacity: {room.Capacity} - Price: {room.PricePerNight:C}");
            }

            Console.Write("Enter the room number you want to Leave: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int selectedRoomNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid room number.");
                return;
            }

            Room roomToLeave = null;
            foreach (var room in unavailableRooms)
            {
                if (room.RoomNumber == selectedRoomNumber)
                {
                    roomToLeave = room;
                    break;
                }
            }



            roomToLeave.Occupied = false;
            roomToLeave.GuestName = "";
            Save();
        }
    }
}
