namespace ProjectHotelRooms
{
    internal class Program
    {
        private static Data data = new Data();
        private static ServiceUI serviceUI = new ServiceUI();
        public static void Main(string[] args)
        {
            serviceUI.DisplayMenu();
            string choice;
            while ((choice = Console.ReadLine()) != "x")
            {
                switch (choice)
                {
                    case "1": //Reservation
                        serviceUI.ReservateRoom();
                        serviceUI.DisplayMenu();
                        break;
                    case "2": //Leave room
                        serviceUI.LeaveRoom();
                        serviceUI.DisplayMenu();
                        break;
                    case "3": //Check avaible rooms and prices
                        serviceUI.DisplayAvaibleRoomUI();
                        serviceUI.DisplayMenu();
                        break;
                    case "4": //Check occupied rooms ant their occupiers
                        serviceUI.DisplayOccupiedRoomUI();
                        serviceUI.DisplayMenu();
                        break;
                    case "5": //Admin panel
                        break;
                    default:
                        Console.WriteLine("Въведохте невалиден избор, моля опитайте отново");
                        Console.WriteLine();
                        serviceUI.DisplayMenu();
                        break;

                }
            }
        }
    }
}
