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
                Rooms = JsonSerializer.Deserialize<List<Room>>(jsonData)!;
            }
            Rooms ??= new List<Room>();
        }
    }
}
