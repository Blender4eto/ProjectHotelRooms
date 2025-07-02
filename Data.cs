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
        public List<Room> DefaultRooms { get; private set; }

        private StreamReader reader;
        private StreamWriter writer;

        public Data()
        {
            LoadRooms();
            LoadDefaultRooms();
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

        public void SaveDefaultRooms()
        {
            writer = new StreamWriter(DefaultRoomsFilePath);
            using (writer)
            {
                string defJsonData = JsonSerializer.Serialize(DefaultRooms);
                writer.Write(defJsonData);
            }
        }

        public void LoadDefaultRooms()
        {
            reader = new StreamReader(DefaultRoomsFilePath);
            using (reader)
            {
                string defJsonData = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(DefaultRoomsFilePath))
                {
                    DefaultRooms = JsonSerializer.Deserialize<List<Room>>(DefaultRoomsFilePath)!;
                }
            }
            DefaultRooms ??= new List<Room>();
        }

        //-------------Display methods----------------

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
