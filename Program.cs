namespace ProjectHotelRooms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ServiceUI serviceUI = new ServiceUI();

            string red = "\u001b[38;2;255;102;102m";
            string green = "\u001b[38;2;102;255;102m";
            string reset = "\u001b[0m";

            serviceUI.DisplayHotelsUI();
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
                    case "5": //Check occupied rooms ant their occupiers
                        serviceUI.DisplayHotelsUI();
                        serviceUI.DisplayMenu();
                        break;
                    case "6": //Admin panel
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
                                    case "1": //Display all rooms
                                        serviceUI.DisplayAllRoomsUI();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "2": //Reservate all free rooms under one guest
                                        serviceUI.ReservateAllFreeRooms();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "3": //Leave all rooms
                                        serviceUI.LeaveAllRooms();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "4": //Add room
                                        serviceUI.AddRoom();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "5": //Edit room
                                        serviceUI.EditRoom();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "6": //Remove room
                                        serviceUI.RemoveRoom();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "7": //Reset rooms to default
                                        serviceUI.ResetToDefaultRooms();
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                    case "8": //Exit admin panel
                                        Console.WriteLine($"{green}Излизане от админ панела...{reset}\n");
                                        serviceUI.DisplayMenu();
                                        exitAdminPanel = true;
                                        break;
                                    default: // Invalid option
                                        Console.Clear();
                                        Console.WriteLine($"{red}Въведохте невалидена опция, моля опитайте отново{reset}");
                                        serviceUI.DisplayAdminMenu();
                                        break;
                                }
                            }
                            break;
                        }
                    default: // Invalid option
                        Console.WriteLine($"{red}Въведохте невалидена опция, моля опитайте отново{reset}\n");
                        serviceUI.DisplayMenu();
                        break;
                }
            }
        }
    }
}
