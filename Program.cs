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
                        {
                            if(!serviceUI.EnterAdminPanel())
                            {
                                serviceUI.DisplayMenu();
                                break;
                            }

                            string adminChoice;
                            bool exitAdminPanel = false;
                            while (!exitAdminPanel)
                            {
                                adminChoice = Console.ReadLine();
                                switch (adminChoice)
                                {
                                    case "1":
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "2":
                                        serviceUI.LeaveAllRooms();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "3":
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "4":
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "5":
                                        serviceUI.ResetToDefaultRooms();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "6":
                                        Console.WriteLine("Излизане от админ панела...");
                                        Console.WriteLine();
                                        serviceUI.DisplayMenu();
                                        exitAdminPanel = true;
                                        break;
                                    default:
                                        Console.WriteLine("Въведохте невалидена опция, моля опитайте отново");
                                        Console.WriteLine();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                }
                            }
                            break;
                        }
                    default:
                        Console.WriteLine("Въведохте невалидена опция, моля опитайте отново");
                        Console.WriteLine();
                        serviceUI.DisplayMenu();
                        break;

                }
            }
        }
    }
}
