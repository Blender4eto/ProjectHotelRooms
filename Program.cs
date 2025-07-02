namespace ProjectHotelRooms
{
    internal class Program
    {
        private static Data data = new Data();
        public static void Main(string[] args)
        {
            string choice;
            while ((choice = Console.ReadLine()) != "x")
            {
                switch (choice)
                {
                    case "1": //Reservation
                        Console.WriteLine("Свободни стаи за резервиране:");
                        ReservateRoom();

                        break;
                    case "2": //Exit from room
                        break;
                    case "3": //Check avaible rooms and prices
                        DisplayAvaibleRoomUI();
                        break;
                    case "4": //check occupied rooms ant their occupiers
                        break;

                }
            }
        }

        private static Room DisplayAvaibleRoomUI()
        {
            throw new NotImplementedException();
        }
        private static Room ReservateRoom()
        {
            throw new NotImplementedException();
        }
    }
}
