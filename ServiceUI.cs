using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectHotelRooms
{
    using static Constants;

    public class ServiceUI
    {
        private static Data data = new Data();
        // TODO: Add denka's BridgeProject Color system
       // TODO: admin panela pri greshen output se dublikira

        public void DisplayHotelLogo()
        {
            // TODO: change logo
            Console.Write("\x1b[38;2;153;204;255m");
            Console.WriteLine("   _  _   __   ____  __    ____  _  _   __  ");
            Console.WriteLine("  / )( \\ / _\\ (  __)(  )  (  __)/ )( \\ /  \\ ");
            Console.WriteLine("  \\ \\/ //    \\ ) _) / (_/\\ ) _) \\ \\/ /(  O )");
            Console.WriteLine("   \\__/ \\_/\\_/(__)  \\____/(____) \\__/  \\__/ ");
            Console.WriteLine("");

            Console.Write("\x1b[0m");
        }


        //UI за избор на хотел
        public void DisplayHotelsUI()
        {
            Console.Clear();
            Console.Write("\x1b[38;2;217;117;177m");
            Console.WriteLine("-----------------Избери Хотел-----------------");
            Console.Write("\x1b[0m");
            Console.WriteLine("1.Черноморец");
            Console.WriteLine("2.Фокус");
            Console.WriteLine("3.Боровец");
            Console.Write("\x1b[38;2;217;117;177m");
            Console.WriteLine("----------------------------------------------");
            Console.Write("\x1b[0m");
            Console.Write("Моля въведете вашия избор: ");

            DisplayHotels();
            Console.Clear();
            Console.Write("\nИзбрахте хотел ");  
            Console.WriteLine($"\x1b[38;2;217;117;177m{data.HotelName}\x1b[0m\n");
        }

        public void DisplayHotels()
        {
            //избор на хотел
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            data.SelectedFilePath = filePath1;
                            data.HotelName = "Черноморец";
                            data.LoadRooms();
                            return;
                        case 2:
                            data.SelectedFilePath = filePath2;
                            data.HotelName = "Фокус";
                            data.LoadRooms();
                            return;
                        case 3:
                            data.SelectedFilePath = filePath3;
                            data.HotelName = "Боровец";
                            data.LoadRooms();
                            return;
                        default:
                            Console.WriteLine("Невалидна опция! Моля изберете 1-3.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Моля въведете число 1-3.");
                }
            }
        }

        //----------------------Main menu------------------------------------

        //UI на главното меню
        public void DisplayMenu()
        {
           
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"-----------------Хотел {data.HotelName}-----------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Списък с предлагани услуги:");
            Console.WriteLine("1. Резервиране на стая");
            Console.WriteLine("2. Освобождаване на стая");
            Console.WriteLine("3. Проверка на наличността и цените на стаите");
            Console.WriteLine("4. Справка за заетите стаи и техните гости");
            Console.WriteLine("5. Смени хотел");
            Console.WriteLine("6. Админ панел");
            Console.WriteLine("x. Изход от програмата");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете вашия избор: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //----------------------Reservate room------------------------------------

        public void ReservateRoom()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"---------Резервиране на стая в {data.HotelName}---------");
            Console.ResetColor();
            Console.WriteLine("Списък със свободни стаи:");
            DisplayAvaibleRoom();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------------------------");
            Console.ResetColor();

            List<Room> availableRooms = data.ReturnAvaibleRooms();

            if (availableRooms.Count == 0) //проверка дали има свободни стаи
            {
                Console.WriteLine("Няма свободни стаи в момента.");
                Console.WriteLine();
                return;
            }

            Console.Write("Моля въведете номера на стаята, която искате да резервирате: ");
            int roomNumber;
            try //проверка дали input-а е валиден
            {
                roomNumber = int.Parse(Console.ReadLine());
            }
            catch //в случай на невалиден input
            {
                Console.Clear();
                Console.WriteLine("Невалиден номер. Операцията е прекратена.");
                Console.WriteLine();
                return;
            }

          
            Room roomToReservate = availableRooms.FirstOrDefault(room => room.RoomNumber == roomNumber); //намиране на стаята по номер

            if (roomToReservate == null) //проверка дали стаята с този номер съществува и дали е свободна
            {
                Console.Clear();
                Console.WriteLine("Стая с този номер не е намерена или вече е заета.");
                Console.WriteLine();
                return;
            }

            //ако номерът е валиден, питаме за името на госта
            Console.Write("Моля въведете името на госта, който резервира стаята: ");
            //написване на името с главна буква
            string guestName = Console.ReadLine().ToLower();
            guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

            //заемане на стаята
            roomToReservate.Occupied = true;
            roomToReservate.GuestName = guestName;
            data.Save();

            Console.Clear();
            Console.WriteLine($"Стая номер {roomToReservate.RoomNumber} е успешно резервирана за {guestName}.");
            Console.WriteLine();
        }
      

        //----------------------Leave room------------------------------------

        public void LeaveRoom()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"---------------Освобождаване на стая в {data.HotelName}--------------");
            Console.ResetColor();

            List<Room> occupiedRooms = data.ReturnOccupiedRooms();
            List<Room> occupiedRoomsByPerson = new List<Room>(); //лист с заетите стаи от конкретен гост

            if (occupiedRooms.Count != 0)
            {
                Console.WriteLine($"Списък със заети стаи в {data.HotelName}:");
                DisplayOccupiedRoom();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--------------------------------------------------");
                Console.ResetColor();

                Console.Write("Моля въведете името на госта, който освобождава стаята: ");
                //написване на името с главна буква
                string guestName = Console.ReadLine().ToLower();
                guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

                Room roomToLeave = null;

                foreach (Room room in data.Rooms)
                {
                    if (room.GuestName == guestName && room.Occupied)
                    {
                        occupiedRoomsByPerson.Add(room);
                    }
                }

                //проверка дали госта притежава повече от една резервация
                if (occupiedRoomsByPerson != null && occupiedRoomsByPerson.Count > 1)
                {
                    Console.WriteLine($"Гостът {guestName} има {occupiedRoomsByPerson.Count} резервации.");
                    Console.WriteLine("Списък с резервациите на госта:");
                    foreach (var room in occupiedRoomsByPerson)
                    {
                        Console.WriteLine($"| Стая номер {room.RoomNumber}, Вид: {room.Type}, Гост: {room.GuestName}");
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("--------------------------------------------------");
                    Console.ResetColor();
                    Console.Write("Въведете коя от резервациите на госта да се освободи: ");
                    
                    int roomNumber;
                    try //извличане на номера на желаната стая за освобождаване
                    { 
                        roomNumber = int.Parse(Console.ReadLine()); 
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Невалиден номер на стая. Моля, опитайте отново.");
                        Console.WriteLine();
                        return;
                    }

                    foreach (Room room in data.Rooms) //проверка за съществуването на стаятя и статуса и
                    {
                        if (room.RoomNumber == roomNumber && room.Occupied)
                        {
                            roomToLeave = room;
                            break;
                        }
                    }

                    if (roomToLeave != null) //освобождаване на стаята
                    {
                        roomToLeave.Occupied = false;
                        roomToLeave.GuestName = null;
                        Console.Clear();
                        Console.WriteLine($"Стая номер {roomToLeave.RoomNumber} е успешно освободена.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Стая с този номер не е намерена или вече е свободна.");
                    }
                }
                //проверка дали госта притежава само една резервация
                else if (occupiedRoomsByPerson != null && occupiedRoomsByPerson.Count == 1)
                {
                    roomToLeave = occupiedRoomsByPerson[0];
                    roomToLeave.Occupied = false;
                    roomToLeave.GuestName = null;
                    Console.Clear();
                    Console.WriteLine($"Стая номер {roomToLeave.RoomNumber} е успешно освободена.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Няма намерени стаи за освобождаване с гост с това име.");
                }
                data.Save();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Няма заети стаи.");
            }
            Console.WriteLine();
        }

        //----------------------All rooms------------------------------------

        //UI на всички стаи
        public void DisplayAllRoomsUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------Списък със всички стаи--------------");
            Console.ResetColor();
            DisplayAllRooms();
        }

        //извеждане на всички стаи
        public void DisplayAllRooms()
        {
            
            List<Room> allRooms = data.ReturnAllRooms();
            if (allRooms.Count == 0)
            {
                Console.WriteLine("Няма налични стаи.");
            }
            else
            {
                foreach (var room in allRooms)
                {
                    if(room.Occupied)
                        Console.WriteLine($"| Стая номер {room.RoomNumber}, Тип: {room.Type}, Цена: {room.PricePerNight} лв. на нощувка, Статус: Заета, Гост: {room.GuestName}");
                    else
                    {
                        Console.WriteLine($"| Стая номер {room.RoomNumber}, Тип: {room.Type}, Цена: {room.PricePerNight} лв. на нощувка, Статус: Свободна, Гост: Няма");
                    }
                }
            }
        }

        //----------------------Avaible rooms------------------------------------

        //UI на свободните стаи
        public void DisplayAvaibleRoomUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------Списък със свободни стаи------------");
            Console.ResetColor();
            DisplayAvaibleRoom();
            Console.WriteLine();
        }

        //извеждане на свободните стаи
        public void DisplayAvaibleRoom()
        {
            List<Room> availableRooms = data.ReturnAvaibleRooms();

            if (availableRooms.Count == 0)
            {
                Console.WriteLine("Няма свободни стаи.");
            }
            else
            {
                foreach (var room in availableRooms)
                {
                    Console.WriteLine($"| Стая номер {room.RoomNumber}, Вид: {room.Type}, Цена: {room.PricePerNight} лв. на нощувка");
                }
            }
        }

        //----------------------Occupied rooms------------------------------------

        //UI на заетите стаи
        public void DisplayOccupiedRoomUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("---------------Списък със заети стаи--------------");
            Console.ResetColor();
            DisplayOccupiedRoom();
            Console.WriteLine();
        }

        //извеждане на заетите стаи
        public void DisplayOccupiedRoom()
        {
            List<Room> occupiedRooms = data.ReturnOccupiedRooms();

            if (occupiedRooms.Count == 0)
            {
                Console.WriteLine("Няма заети стаи.");
            }
            else
            {
                foreach (var room in occupiedRooms)
                {
                    Console.WriteLine($"| Стая номер {room.RoomNumber}, Вид: {room.Type}, Гост: {room.GuestName}");
                }
            }
        }

        //----------------------Admin Panel------------------------------------

        public const string AdminPassword = "salam"; //парола за админ панела

        //проверка на паролата
        public bool EnterAdminPanel()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-------------------Админ панел--------------------");
            Console.ResetColor();
            Console.Write("Въведете парола: ");
            if (Console.ReadLine() == AdminPassword)
            {
                Console.Clear();
                DisplayAdminMenu();
                return true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Невалидна парола, вие ще бъдете върнати в главното меню.");
                Console.WriteLine();
                return false;
            }
        }


        //UI на админ менюто
        public void DisplayAdminMenu()
        {
           
            Console.Write("\x1b[38;2;217;117;177m");
            Console.WriteLine();
            Console.WriteLine($"---------Aдмин панела на хотел {data.HotelName}---------");
            Console.Write("\x1b[0m");
            Console.WriteLine("1. Справка за всички стаи");
            Console.WriteLine("2. Резервиране на всички стаи");
            Console.WriteLine("3. Освобождаване на всички стаи");
            Console.WriteLine("4. Добавяне на стая");
            Console.WriteLine("5. Редактиране на стая");
            Console.WriteLine("6. Премахване на стая");
            Console.WriteLine("7. Връщане на списъка на стаите по умолчание");
            Console.WriteLine("8. Изход от админ панела");
            Console.Write("\x1b[38;2;217;117;177m");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("\x1b[0m");
            Console.Write("Моля въведете вашия избор: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //----------------------Reservate all rooms------------------------------------

        //резервиране на всички свободни стаи
        public void ReservateAllFreeRooms()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"-------Резервиране на всички свободни стаи в {data.HotelName}--------");

            List<Room> availableRooms = data.ReturnAvaibleRooms(); //списък със свободни стаи
            string guestName;

            if (availableRooms.Count != 0) //проверка дали има свободни стаи
            {
                Console.Write("Моля въведете името на госта, който резервира свободните стаи: ");
                //написване на името с главна буква
                guestName = Console.ReadLine().ToLower();

                if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
                {
                    Console.Clear();
                    Console.WriteLine("Името на госта не може да бъде празно. Операцията е прекратена.");
                    return;
                }
                guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

                foreach (var room in data.Rooms) //резервиране на всички свободни стаи
                {
                    if (!room.Occupied)
                    {
                        room.Occupied = true;
                        room.GuestName = guestName;
                    }
                }
                Console.Clear();
                Console.WriteLine($"Всички свободни стаи са резервирани от {guestName}.");
                data.Save();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Няма свободни стаи за резервиране.");
                return;
            }
        }

        //----------------------Leave all rooms------------------------------------

        //освобождаване на всички стаи
        public void LeaveAllRooms()
        {
            Console.Clear();
            // TODO: add a option to leave selected rooms + option to kick out guest and leave all his rooms
            //TODO: check if any of the rooms are already empty

            Console.WriteLine();
            Console.WriteLine($"-----------Освобождаване на всички стаи в {data.HotelName}-----------");

            foreach (var room in data.Rooms) //освобождаване на всички стаи
            {
                room.Occupied = false;
                room.GuestName = null;
            }
            Console.Clear();
            Console.WriteLine("Всички стаи са освободени.");
            data.Save();
        }

        //----------------------Add room------------------------------------

        public void AddRoom()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("--------------Създаване на нова стая в --------------");

            //Добавяне на номера на стаята
            Console.Write("Моля въведете номера на стаята: ");
            int roomNumber;
            try //проверка дали input-а е валиден
            {
                roomNumber = int.Parse(Console.ReadLine());
                if (data.Rooms.Any(r => r.RoomNumber == roomNumber)) //проверка дали стаята с този номер вече съществува
                {
                    Console.Clear();
                    Console.WriteLine("Стая с този номер вече съществува. Операцията е прекратена.");
                    return;
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                return;
            }

            //добавяне на типа на стаята
            Console.WriteLine("Видове стаи:");
            Console.WriteLine("1. Единична");
            Console.WriteLine("2. Двойна");
            Console.WriteLine("3. Луксозна");
            Console.WriteLine("4. Апартамент");
            Console.WriteLine("5. Фамилна");
            Console.WriteLine("6. Друг");
            Console.Write("Моля въведете типа на стаята: ");
            string type = Console.ReadLine()?.ToLower();
            int capacity;
            switch (type)
            {
                case "1":
                    type = "Единична";
                    capacity = 1;
                    break;
                case "2":
                    type = "Двойна";
                    capacity = 2;
                    break;
                case "3":
                    type = "Луксозна";
                    capacity = 3;
                    break;
                case "4":
                    type = "Апартамент";
                    capacity = 4;
                    break;
                case "5":
                    type = "Фамилна";
                    capacity = 5;
                    break;
                case "6":
                    Console.Write("Моля въведете вашия тип на стаята: ");
                    //написване на вида с главна буква
                    type = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(type) || type.All(char.IsDigit)) //проверка дали input-а е валиден
                    {
                        Console.Clear();
                        Console.WriteLine("Типът на стаята не може да бъде празен или да съдържа число. Операцията е прекратена.");
                        return;
                    }
                    type = char.ToUpper(type[0]) + type.Substring(1);

                    //добавяне на капацитета на стаята ако е избран "Друг" (понеже на другите типове е зададен капацитет)
                    Console.Write("Моля въведете номер на капацитета на стаята: ");
                    try //проверка дали input-а е валиден
                    {
                        capacity = int.Parse(Console.ReadLine());
                        if (capacity <= 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Капацитетът на стаята трябва да бъде положително число. Операцията е прекратена.");
                            return;
                        }
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Невалиден капацитет на стаята. Операцията е прекратена.");
                        Console.WriteLine();
                        return;
                    }
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Невалиден тип на стаята. Операцията е прекратена.");
                    return;
            }


            //добавяне на цената на стаята
            Console.Write("Моля въведете цената на стаята за нощувка (XX.XX): ");
            decimal pricePerNight;
            try //проверка дали input-а е валиден
            {
                pricePerNight = decimal.Parse(Console.ReadLine());
                if (pricePerNight <= 0) //проверка дали цената е положително число
                {
                    Console.Clear();
                    Console.WriteLine("Цената на стаята трябва да бъде положително число. Операцията е прекратена.");
                    return;
                }
                pricePerNight = Math.Round(pricePerNight, 2); //закръглане до 2 знака след десетичната запетая (не работи напълно)
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Невалидна цена на стаята. Операцията е прекратена.");
                Console.WriteLine();
                return;
            }

            //добавяне на статуса на стаята
            Console.Write("Моля въведете дали стаята да е заета (да/не): ");
            bool isOccupied;
            string guestName;
            switch (Console.ReadLine()?.ToLower())
            {
                //в случай че се използва друга клавиатура
                case "да":
                case "da":
                case "yes":
                    isOccupied = true;
                    //добавяне на името на госта
                    Console.Write("Моля въведете името на госта, който ще заеме стаята: ");
                    guestName = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
                    {
                        Console.Clear();
                        Console.WriteLine("Името на госта не може да бъде празно. Операцията е прекратена.");
                        return;
                    }
                    guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

                    break;
                //в случай че се използва друга клавиатура
                case "не":
                case "ne":
                case "no":
                    isOccupied = false;
                    guestName = null;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Невалиден отговор. Стаята ще бъде добавена като свободна.");
                    isOccupied = false;
                    guestName = null;
                    break;
            }

            //добавяне на новата стая
            Room newRoom = new Room(roomNumber, type, capacity, pricePerNight, isOccupied, guestName);
            data.Rooms.Add(newRoom);
            data.Save();

            Console.Clear();
            Console.WriteLine($"Стая номер {newRoom.RoomNumber} е успешно добавена.");
        }

        //----------------------Edit room------------------------------------

        public void EditRoom()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("---------------Редактиране на стая----------------");
            Console.WriteLine("Списък със стаи:");
            DisplayAllRooms();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Въведете номера на стаята, която желаете да редактирате: ");

            int roomNumber;
            Room roomToEdit = null;
            try //проверка дали input-а е валиден
            {
                roomNumber = int.Parse(Console.ReadLine());
                foreach (Room room in data.Rooms)
                {
                    if (room.RoomNumber == roomNumber) //намиране на стаята по номер
                    {
                        roomToEdit = room;
                        break;
                    }
                }
                if (roomToEdit == null)
                {
                    Console.Clear();
                    Console.WriteLine("Стая с този номер не съществува. Операцията е прекратена.");
                    return;
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                return;
            }

            Console.WriteLine("Какво искате да редактирате?");
            Console.WriteLine("1. Номера на стаята");
            Console.WriteLine("2. Типа на стаята");
            Console.WriteLine("3. Цената на стаята за нощувка");
            Console.WriteLine("4. Статуса на стаята");
            Console.WriteLine("5. Името на госта");
            Console.WriteLine("6. Изход от редактирането на стаята");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете вашия избор: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": //Edit room number
                    Console.Write("Моля въведете новия номер на стаята: ");
                    try //проверка дали input-а е валиден
                    {
                        int newRoomNumber = int.Parse(Console.ReadLine());
                        if (data.Rooms.Any(r => r.RoomNumber == newRoomNumber)) //проверка дали новия номер на стаята вече съществува
                        {
                            Console.Clear();
                            Console.WriteLine("Стая с този номер вече съществува. Операцията е прекратена.");
                            return;
                        }
                        roomToEdit.RoomNumber = newRoomNumber;
                        Console.Clear();
                        Console.WriteLine($"Номера на стаята е успешно променен на {newRoomNumber}.");
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                        return;
                    }
                    break;
                case "2": //Edit room type
                    Console.WriteLine("Видове стаи:");
                    Console.WriteLine("1. Единична");
                    Console.WriteLine("2. Двойна");
                    Console.WriteLine("3. Луксозна");
                    Console.WriteLine("4. Апартамент");
                    Console.WriteLine("5. Фамилна");
                    Console.WriteLine("6. Друг");
                    Console.WriteLine("--------------------------------------------------");
                    Console.Write("Моля въведете новия тип на стаята: ");
                    string newType = Console.ReadLine()?.ToLower();

                    int newCapacity;
                    switch (newType)
                    {
                        case "1":
                            newType = "Единична";
                            break;
                        case "2":
                            newType = "Двойна";
                            break;
                        case "3":
                            newType = "Луксозна";
                            break;
                        case "4":
                            newType = "Апартамент";
                            break;
                        case "5":
                            newType = "Фамилна";
                            break;
                        case "6":
                            Console.Write("Моля въведете вашия тип на стаята: ");
                            //написване на вида с главна буква
                            newType = Console.ReadLine()?.ToLower();

                            if (string.IsNullOrEmpty(newType) || newType.All(char.IsDigit)) //проверка дали input-а е валиден
                            {
                                Console.Clear();
                                Console.WriteLine("Типът на стаята не може да бъде празен или да съдържа число. Операцията е прекратена.");
                                return;
                            }
                            newType = char.ToUpper(newType[0]) + newType.Substring(1);

                            Console.Write("Моля въведете новия капацитет на стаята: ");
                            try //проверка дали input-а е валиден
                            {
                                newCapacity = int.Parse(Console.ReadLine());
                                if (newCapacity <= 0) //проверка дали капацитета е положително число
                                {
                                    Console.Clear();
                                    Console.WriteLine("Капацитетът на стаята трябва да бъде положително число. Операцията е прекратена.");
                                    return;
                                }
                                roomToEdit.Capacity = newCapacity;
                                data.Save();

                                Console.Clear();
                                Console.WriteLine($"Капацитетът на стаята е успешно променен на {newCapacity}.");
                            }
                            catch
                            {
                                Console.Clear();
                                Console.WriteLine("Невалиден капацитет на стаята. Операцията е прекратена.");
                                return;
                            }
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Невалиден тип на стаята. Операцията е прекратена.");
                            return;
                    }
                    roomToEdit.Type = newType;
                    data.Save();

                    Console.Clear();
                    Console.WriteLine($"Типът на стаята е успешно променен на {newType}.");
                    break;
                case "3": //Edit room price
                    Console.Write("Моля въведете новата цена на стаята за нощувка (XX.XX): ");
                    decimal newPricePerNight;
                    try //проверка дали input-а е валиден
                    {
                        newPricePerNight = decimal.Parse(Console.ReadLine());
                        if (newPricePerNight <= 0) //проверка дали цената е положително число
                        {
                            Console.Clear();
                            Console.WriteLine("Цената на стаята трябва да бъде положително число. Операцията е прекратена.");
                            return;
                        }
                        newPricePerNight = Math.Round(newPricePerNight, 2); //закръглане до 2 знака след десетичната запетая (не работи напълно)
                        roomToEdit.PricePerNight = newPricePerNight;
                        data.Save();

                        Console.Clear();
                        Console.WriteLine($"Цената на стаята е успешно променена на {newPricePerNight} лв. на нощувка.");
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Невалидна цена на стаята. Операцията е прекратена.");
                        return;
                    }
                    break;
                case "4": //Edit room status
                    if (roomToEdit.Occupied) //проверка дали сатята е заета
                    {
                        roomToEdit.Occupied = false;
                        roomToEdit.GuestName = null;
                        Console.Clear();
                        Console.WriteLine($"Стая номер {roomToEdit.RoomNumber} е успешно освободена.");
                    }
                    else
                    {
                        roomToEdit.Occupied = true;
                        Console.Write("Моля въведете името на госта, който ще заеме стаята: ");
                        //написване на името с главна буква
                        string guestName = Console.ReadLine()?.ToLower();

                        if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
                        {
                            Console.Clear();
                            Console.WriteLine("Името на госта не може да бъде празно. Операцията е прекратена.");
                            return;
                        }
                        guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);
                        roomToEdit.GuestName = guestName; //промяна на името на госта ако стаята се резервира

                        Console.Clear();
                        Console.WriteLine($"Стая номер {roomToEdit.RoomNumber} е успешно резервирана за {guestName}.");
                    }
                    data.Save();
                    break;

                case "5": //Edit guest name
                    if (roomToEdit.Occupied) //проверка дали сатята е заета
                    {
                        Console.Write("Моля въведете новото име на госта: ");
                        //написване на името с главна буква
                        string newGuestName = Console.ReadLine()?.ToLower();

                        if (string.IsNullOrEmpty(newGuestName)) //проверка дали името на госта е празно
                        {
                            Console.Clear();
                            Console.WriteLine("Името на госта не може да бъде празно. Операцията е прекратена.");
                            return;
                        }
                        newGuestName = char.ToUpper(newGuestName[0]) + newGuestName.Substring(1);
                        roomToEdit.GuestName = newGuestName; //промяна на името на госта

                        Console.Clear();
                        Console.WriteLine($"Името на госта е успешно променено на {newGuestName}.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Стаята не е заета. Няма нужда от промяна на името на госта.");
                    }
                    data.Save();
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine("Изход от редактирането на стаята.");
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Невалиден избор. Операцията е прекратена.");
                    return;
            }
        }

        //----------------------Remove room------------------------------------

        //премахване на стая
        public void RemoveRoom()
        {
            Console.WriteLine();
            Console.WriteLine("----------------Премахване на стая----------------");
            Console.WriteLine("Списък със стаи:");
            DisplayAllRooms();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Въведете номера на стаята, която желаете да премахнете: ");
            int roomNumber;
            try //проверка дали input-а е валиден
            {
                roomNumber = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                return;
            }

            if (!data.Rooms.Any(r => r.RoomNumber == roomNumber)) //проверка дали стаята с този номер съществува
            {
                Console.Clear();
                Console.WriteLine("Стая с този номер не съществува. Операцията е прекратена.");
                return;
            }
            foreach (Room room in data.Rooms)
            {
                if (room.RoomNumber == roomNumber) //намиране на стаята по номер
                {
                    data.Rooms.Remove(room);
                    data.Save();

                    Console.Clear();
                    Console.WriteLine($"Стая номер {room.RoomNumber} е успешно премахната.");
                    return;
                }
            }
        }

        //----------------------Return to default rooms------------------------------------

        //възстановяване на списъка със стаите по умолчание
        public void ResetToDefaultRooms()
        {
            Console.Write("Сигурни ли сте, че желаете да възстановите списъка със стаите (да/не): ");
            string confirmation = Console.ReadLine()?.ToLower();
            switch (confirmation)
            {
                //в случай че се използва друга клавиатура
                case "да":
                case "da":
                case "yes":
                    data.ResetRoomsToDefault();
                    Console.Clear();
                    Console.WriteLine("Възстановяване на списъка...");
                    break;
                //в случай че се използва друга клавиатура
                case "не":
                case "ne":
                case "no":
                    Console.Clear();
                    Console.WriteLine("Възстановяването е отменено.");
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Невалиден отговор. Вие ще бъдете върнати в админ панела.");
                    break;
            }
        }
    }
}
