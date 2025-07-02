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
                DisplayMenu();
                switch (choice)
                {
                    case "1": //Reservation
                        Console.WriteLine("Свободни стаи за резервиране:");
                        data.ReservateRoom();
                        data.Save();
                        break;
                    case "2": //Exit from room
                        data.ExitRoom();
                        data.Save();
                        break;
                    case "3": //Check avaible rooms and prices
                        data.DisplayRoomsAvaliable();
                        break;
                    case "4": //check occupied rooms ant their occupiers
                        data.DisplayRoomsUnavaliable();
                        break;

                    case "5": //check occupied rooms ant their occupiers
                        data.DisplayRoomsAll();
                        break;

                }
            }
        }
        private static void DisplayMenu()
        {
            // TODO: Implement application menu
        }
        private static Room ReservateRoom()
        {
            // TODO: Implement list with avaible rooms and choose number for one of them to reserve
            throw new NotImplementedException();
        }
        private static Room ExitRoom()
        {
            // TODO: Implement list with occupied rooms, enter your name, find the room you want to exit via name
            throw new NotImplementedException();
        }

        private static Room DisplayAvaibleRoomUI()
        {
            // TODO: Implement lsit with avaible rooms
            //better use where;
            throw new NotImplementedException();
        }

        private static Room DisplayOccupiedRoomUI()
        {
            // TODO: Implement lsit with occupied rooms and their owners
            //better use where;
            throw new NotImplementedException();
        }

    }
}