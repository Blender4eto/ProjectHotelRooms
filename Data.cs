using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace ProjectHotelRooms
{
    using System.Text.Json;
    using static Constants;
    using static ServiceUI;
    public  class Data
    {
        

        public List<Room> Rooms { get; private set; }
        public List<Room> DefaultRooms { get; private set; }
        public string SelectedFilePath = filePath3;
        public string HotelName = "";

        private StreamReader reader;
        private StreamWriter writer;

        public Data()
        {
            
            LoadRooms();
        }

        //-------------Main file operations----------------
        public void DisplayHotels()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("-----------------Избери Хотел-----------------");
            Console.WriteLine("1.Черноморец");
            Console.WriteLine("2. Фокус");
            Console.WriteLine("3. Боровец");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете вашия избор: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        SelectedFilePath = filePath1;
                        HotelName = "Черноморец";
                        break;
                    case 2:
                        SelectedFilePath = filePath2;
                        HotelName = "Фокус";
                        break;
                    case 3:
                        SelectedFilePath = filePath3;
                        HotelName = "Боровец";
                        break;
                    default:
                        Console.WriteLine("Невалидна опция! Моля изберете 1-3.");
                        break;
                }
                // Reload rooms after changing hotel
                LoadRooms();
            }
            else
            {
                Console.WriteLine("Невалиден вход! Моля въведете число.");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(SelectedFilePath);
           
        }
        public void Save()
        {
            StreamWriter writer = new StreamWriter(SelectedFilePath);
            using (writer)
            {                                                       
                string jsonData = JsonSerializer.Serialize(Rooms);
                writer.Write(jsonData);
            }
        }

        public void LoadRooms()
        {
            reader = new StreamReader(SelectedFilePath);
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

        //-------------Default file operations----------------

        public void ResetRoomsToDefault()
        {
            File.Copy(DefaultRoomsFilePath, SelectedFilePath, overwrite: true);
            LoadRooms();
        }

        //-------------Display methods----------------
        public List<Room> DisplayAllRooms()
        {
            return Rooms;
        }

        public List<Room> DisplayAvaibleRooms()
        {
            return Rooms.Where(r => !r.Occupied).ToList();
        }

        public List<Room> DisplayOccupiedRooms()
        {
            return Rooms.Where(r => r.Occupied).ToList();
        }
    }
}
