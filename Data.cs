namespace ProjectHotelRooms
{
    using System.Text.Json;
    using static Constants;
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
        public List<Room> ReturnAllRooms()
        {
            return Rooms;
        }

        public List<Room> ReturnAvaibleRooms()
        {
            return Rooms.Where(r => !r.Occupied).ToList();
        }

        public List<Room> ReturnOccupiedRooms()
        {
            return Rooms.Where(r => r.Occupied).ToList();
        }
    }
}
