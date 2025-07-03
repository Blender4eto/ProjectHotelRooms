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
    public  class Data
    {
        public List<Room> Rooms { get; private set; }
        public List<Room> DefaultRooms { get; private set; }

        private StreamReader reader;
        private StreamWriter writer;

        public Data()
        {
            LoadRooms();
        }

        //-------------Main file operations----------------

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

        //-------------Default file operations----------------

        public void ResetRoomsToDefault()
        {
            File.Copy(DefaultRoomsFilePath, filePath, overwrite: true);
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
