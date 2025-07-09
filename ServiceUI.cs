namespace ProjectHotelRooms
{
    using static Constants;

    public class ServiceUI
    {
        private static Data data = new Data();

        //цветове за конзолата
        public string blue = "\u001b[38;2;153;204;255m";
        public string cyan = "\u001b[38;2;102;255;255m";
        public string pink = "\u001b[38;2;217;117;177m";
        public string yellow = "\u001b[38;2;255;255;102m";
        public string green = "\u001b[38;2;102;255;102m";
        public string red = "\u001b[38;2;255;102;102m";
        public string reset = "\u001b[38;2;255;255;255m";

        //UI за избор на хотел
        public void DisplayHotelsUI()
        {
            Console.Clear();
            Console.WriteLine($"{pink}-----------------Избери Хотел-----------------{reset}");
            Console.WriteLine("1. Черноморец");
            Console.WriteLine("2. Споко");
            Console.WriteLine("3. Боровец");
            Console.WriteLine($"{pink}----------------------------------------------{reset}");
            Console.Write($"{reset}Моля въведете вашия избор: {pink}");

            ChooseHotels();
            Console.Clear();
            Console.Write($"{green}Избрахте хотел {pink}{data.HotelName}{reset}\n \n");
        }

        public void ChooseHotels()
        {
            //избор на хотел
            while (true)
            {
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        data.SelectedFilePath = filePath1;
                        data.HotelName = "Черноморец";
                        data.LoadRooms();
                        return;
                    case "2":
                        data.SelectedFilePath = filePath2;
                        data.HotelName = "Споко";
                        data.LoadRooms();
                        return;
                    case "3":
                        data.SelectedFilePath = filePath3;
                        data.HotelName = "Боровец";
                        data.LoadRooms();
                        return;
                    default:
                        Console.Write($"\n{red}Невалидна опция! Моля изберете 1-3.{reset}\n");
                        Console.Write($"{reset}Опитайте отново: {pink}");
                        break;
                }
            }
        }

        //----------------------Main menu------------------------------------

        //UI на главното меню
        public void DisplayMenu()
        {
            Console.WriteLine($"{yellow}-----------------Хотел {data.HotelName}-----------------{reset}");
            Console.WriteLine($"{cyan}Списък с предлагани услуги:{reset}");
            Console.WriteLine("1. Резервиране на стая");
            Console.WriteLine("2. Освобождаване на стая");
            Console.WriteLine("3. Проверка на наличността и цените на стаите");
            Console.WriteLine("4. Справка за заетите стаи и техните гости");
            Console.WriteLine("5. Смени хотел");
            Console.WriteLine("6. Админ панел");
            Console.WriteLine($"x. Изход от програмата");
            Console.WriteLine($"{yellow}--------------------------------------------------{reset}");
            Console.Write($"{reset}Моля въведете вашия избор: {pink}");
        }

        //----------------------Reservate room------------------------------------

        public void ReservateRoom()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}---------Резервиране на стая---------{reset}");
            Console.WriteLine($"{cyan}Списък със свободни стаи:{reset}");
            DisplayAvaibleRoom();
            Console.WriteLine($"{cyan}--------------------------------------------------{reset}");

            List<Room> availableRooms = data.ReturnAvaibleRooms();

            if (availableRooms.Count == 0) //проверка дали има свободни стаи
            {
                Console.WriteLine($"{red}Няма свободни стаи в момента.{reset}\n");
                return;
            }

            Console.Write($"{reset}Моля въведете номера на стаята, която искате да резервирате: {pink}");
            int roomNumber;
            try //проверка дали input-а е валиден
            {
                roomNumber = int.Parse(Console.ReadLine());
            }
            catch //в случай на невалиден input
            {
                Console.Clear();
                Console.WriteLine($"{red}Невалиден номер. Операцията е прекратена.{reset}\n");
                return;
            }

          
            Room roomToReservate = availableRooms.FirstOrDefault(room => room.RoomNumber == roomNumber); //намиране на стаята по номер

            if (roomToReservate == null) //проверка дали стаята с този номер съществува и дали е свободна
            {
                Console.Clear();
                Console.WriteLine($"{red}Стая с този номер не е намерена или вече е заета.{reset}\n");
                return;
            }

            //ако номерът е валиден, питаме за името на госта
            Console.Write($"{reset}Моля въведете името на госта, който резервира стаята: {pink}");
            //написване на името с главна буква
            string guestName = Console.ReadLine().ToLower();
            if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
            {
                Console.Clear();
                Console.WriteLine($"{red}Името на госта не може да бъде празно. Операцията е прекратена.{reset}\n");
                return;
            }
            guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

            //заемане на стаята
            roomToReservate.Occupied = true;
            roomToReservate.GuestName = guestName;
            data.Save();

            Console.Clear();
            Console.WriteLine($"{green}Стая номер {pink}{roomToReservate.RoomNumber}{green} е успешно резервирана за {pink}{guestName}{green}.{reset}");
            Console.WriteLine();
        }
      

        //----------------------Leave room------------------------------------

        public void LeaveRoom()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}---------------Освобождаване на стая--------------{reset}");

            List<Room> occupiedRooms = data.ReturnOccupiedRooms();
            List<Room> occupiedRoomsByPerson = new List<Room>(); //лист с заетите стаи от конкретен гост

            if (occupiedRooms.Count != 0)
            {
                Console.WriteLine($"{cyan}Списък със заети стаи:{reset}");
                DisplayOccupiedRoom();
                Console.WriteLine($"{cyan}--------------------------------------------------{reset}");

                Console.Write($"{reset}Моля въведете името на госта, който освобождава стаята: {pink}");
                //написване на името с главна буква
                string guestName = Console.ReadLine().ToLower();
                if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
                {
                    Console.Clear();
                    Console.WriteLine($"{red}Името на госта не може да бъде празно. Операцията е прекратена.{reset}\n");
                    return;
                }
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
                    Console.WriteLine($"\n{reset}Гостът {pink}{guestName}{reset} има {pink}{occupiedRoomsByPerson.Count}{reset} резервации.");
                    Console.WriteLine($"{cyan}Списък с резервациите на госта:{reset}");
                    foreach (var room in occupiedRoomsByPerson)
                    {
                        Console.WriteLine($"| Стая номер {pink}{room.RoomNumber}{reset}, Вид: {pink}{room.Type}{reset}, Гост: {pink}{room.GuestName}{reset}");
                    }
                    Console.WriteLine($"{cyan}--------------------------------------------------{reset}");
                    Console.Write($"{reset}Въведете коя от резервациите на госта да се освободи: {pink}");
                    
                    int roomNumber;
                    try //извличане на номера на желаната стая за освобождаване
                    { 
                        roomNumber = int.Parse(Console.ReadLine()); 
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Невалиден номер на стая. Моля, опитайте отново.{reset}\n");
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
                        Console.WriteLine($"{green}Стая номер {pink}{roomToLeave.RoomNumber}{green} е успешно освободена.{reset}");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Стая с този номер не е намерена или вече е свободна.{reset}");
                    }
                }
                //проверка дали госта притежава само една резервация
                else if (occupiedRoomsByPerson != null && occupiedRoomsByPerson.Count == 1)
                {
                    roomToLeave = occupiedRoomsByPerson[0];
                    roomToLeave.Occupied = false;
                    roomToLeave.GuestName = null;
                    Console.Clear();
                    Console.WriteLine($"{green}Стая номер {pink}{roomToLeave.RoomNumber}{green} е успешно освободена.{reset}");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"{red}Няма намерени стаи за освобождаване с гост с това име.{reset}");
                }
                data.Save();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{red}Няма заети стаи.{reset}");
            }
            Console.WriteLine();
        }

        //----------------------All rooms------------------------------------

        //UI на всички стаи
        public void DisplayAllRoomsUI()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}--------------Списък със всички стаи--------------{reset}");
            DisplayAllRooms();
        }

        //извеждане на всички стаи
        public void DisplayAllRooms()
        {
            
            List<Room> allRooms = data.ReturnAllRooms();
            if (allRooms.Count == 0)
            {
                Console.WriteLine($"{red}Няма налични стаи.{reset}");
            }
            else
            {
                foreach (var room in allRooms)
                {
                    if(room.Occupied)
                        Console.WriteLine($"| Стая номер {pink}{room.RoomNumber}{reset}, Тип: {pink}{room.Type}{reset}, Цена: {pink}{room.PricePerNight}{reset} лв. на нощувка, Статус: Заета, Гост: {pink}{room.GuestName}{reset}");
                    else
                    {
                        Console.WriteLine($"| Стая номер {pink}{room.RoomNumber}{reset}, Тип: {pink}{room.Type}{reset}, Цена: {pink}{room.PricePerNight}{reset} лв. на нощувка, Статус: Свободна, Гост: Няма");
                    }
                }
            }
        }

        //----------------------Avaible rooms------------------------------------

        //UI на свободните стаи
        public void DisplayAvaibleRoomUI()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}--------------Списък със свободни стаи------------{reset}");
            DisplayAvaibleRoom();
            Console.WriteLine();
        }

        //извеждане на свободните стаи
        public void DisplayAvaibleRoom()
        {
            List<Room> availableRooms = data.ReturnAvaibleRooms();

            if (availableRooms.Count == 0)
            {
                Console.WriteLine($"{red}Няма свободни стаи.{reset}");
            }
            else
            {
                foreach (var room in availableRooms)
                {
                    Console.WriteLine($"| Стая номер {pink}{room.RoomNumber}{reset}, Вид: {pink}{room.Type}{reset}, Цена: {pink}{room.PricePerNight}{reset} лв. на нощувка");
                }
            }
        }

        //----------------------Occupied rooms------------------------------------

        //UI на заетите стаи
        public void DisplayOccupiedRoomUI()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}---------------Списък със заети стаи--------------{reset}");
            DisplayOccupiedRoom();
            Console.WriteLine();
        }

        //извеждане на заетите стаи
        public void DisplayOccupiedRoom()
        {
            List<Room> occupiedRooms = data.ReturnOccupiedRooms();

            if (occupiedRooms.Count == 0)
            {
                Console.WriteLine($"{red}Няма заети стаи.{reset}");
            }
            else
            {
                foreach (var room in occupiedRooms)
                {
                    Console.WriteLine($"| Стая номер {pink}{room.RoomNumber}{reset}, Вид: {pink}{room.Type}{reset}, Гост: {pink}{room.GuestName}{reset}");
                }
            }
        }

        //----------------------Admin Panel------------------------------------

        public const string AdminPassword = "salam"; //парола за админ панела

        //проверка на паролата
        public bool EnterAdminPanel()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}-------------------Админ панел--------------------{reset}");
            Console.Write($"{reset}Въведете парола: {pink}");
            if (Console.ReadLine() == AdminPassword)
            {
                Console.Clear();
                DisplayAdminMenu();
                return true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{red}Невалидна парола, вие ще бъдете върнати в главното меню.{reset}");
                Console.WriteLine();
                return false;
            }
        }


        //UI на админ менюто
        public void DisplayAdminMenu()
        {
            Console.WriteLine($"\n{pink}---------Aдмин панел на хотел {data.HotelName}---------{reset}");
            Console.WriteLine("1. Справка за всички стаи");
            Console.WriteLine("2. Резервиране на всички стаи");
            Console.WriteLine("3. Освобождаване на всички стаи");
            Console.WriteLine("4. Добавяне на стая");
            Console.WriteLine("5. Редактиране на стая");
            Console.WriteLine("6. Премахване на стая");
            Console.WriteLine("7. Връщане на списъка на стаите по умолчание");
            Console.WriteLine("8. Изход от админ панела");
            Console.WriteLine($"{pink}--------------------------------------------------{reset}");
            Console.Write($"{reset}Моля въведете вашия избор: {pink}");
        }

        //----------------------Reservate all rooms------------------------------------

        //резервиране на всички свободни стаи
        public void ReservateAllFreeRooms()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}-------Резервиране на всички свободни стаи в {data.HotelName}--------{reset}");

            List<Room> availableRooms = data.ReturnAvaibleRooms(); //списък със свободни стаи
            string guestName;

            if (availableRooms.Count != 0) //проверка дали има свободни стаи
            {
                Console.Write($"{reset}Моля въведете името на госта, който резервира свободните стаи: {pink}");
                //написване на името с главна буква
                guestName = Console.ReadLine().ToLower();

                if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
                {
                    Console.Clear();
                    Console.WriteLine($"{red}Името на госта не може да бъде празно. Операцията е прекратена.{reset}");
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
                Console.WriteLine($"{green}Всички свободни стаи са резервирани от {pink}{guestName}{green}.{reset}");
                data.Save();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{red}Няма свободни стаи за резервиране.{reset}");
                return;
            }
        }

        //----------------------Leave all rooms------------------------------------

        //освобождаване на всички стаи
        public void LeaveAllRooms()
        {
            Console.Clear();

            List<Room> occupiedRooms = data.ReturnOccupiedRooms(); //списък със заети стаи
            if (occupiedRooms.Count != 0) //проверка дали има заети стаи
            {
                foreach (var room in data.Rooms) //освобождаване на всички стаи
                {
                    room.Occupied = false;
                    room.GuestName = null;
                }
                Console.Clear();
                Console.WriteLine($"{green}Всички стаи са освободени.{reset}");
                data.Save();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{red}Няма заети стаи за освобождаване.{reset}");
            }
        }

        //----------------------Add room------------------------------------

        //добавяне на стая
        public void AddRoom()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}--------------Създаване на нова стая--------------{reset}");

            //Добавяне на номера на стаята
            Console.Write($"{reset}Моля въведете номера на стаята: {pink}");
            int roomNumber;
            try //проверка дали input-а е валиден
            {
                roomNumber = int.Parse(Console.ReadLine());
                if (data.Rooms.Any(r => r.RoomNumber == roomNumber)) //проверка дали стаята с този номер вече съществува
                {
                    Console.Clear();
                    Console.WriteLine($"{red}Стая с този номер вече съществува. Операцията е прекратена.{reset}");
                    return;
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine($"{red}Невалиден номер на стая. Операцията е прекратена.{reset}");
                return;
            }

            //добавяне на типа на стаята
            Console.WriteLine($"{cyan}Видове стаи:{reset}");
            Console.WriteLine("1. Единична");
            Console.WriteLine("2. Двойна");
            Console.WriteLine("3. Луксозна");
            Console.WriteLine("4. Апартамент");
            Console.WriteLine("5. Фамилна");
            Console.WriteLine("6. Друг");
            Console.WriteLine($"{cyan}------------{reset}");
            Console.Write($"{reset}Моля въведете типа на стаята: {pink}");
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
                    Console.Write($"{reset}Моля въведете вашия тип на стаята: {pink}");
                    //написване на вида с главна буква
                    type = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(type) || type.All(char.IsDigit)) //проверка дали input-а е валиден
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Типът на стаята не може да бъде празен или да съдържа число. Операцията е прекратена.{reset}");
                        return;
                    }
                    type = char.ToUpper(type[0]) + type.Substring(1);

                    //добавяне на капацитета на стаята ако е избран "Друг" (понеже на другите типове е зададен капацитет)
                    Console.Write($"{reset}Моля въведете номер на капацитета на стаята: {pink}");
                    try //проверка дали input-а е валиден
                    {
                        capacity = int.Parse(Console.ReadLine());
                        if (capacity <= 0)
                        {
                            Console.Clear();
                            Console.WriteLine($"{red}Капацитетът на стаята трябва да бъде положително число. Операцията е прекратена.{reset}");
                            return;
                        }
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Невалиден капацитет на стаята. Операцията е прекратена.{reset}");
                        return;
                    }
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine($"{red}Невалиден тип на стаята. Операцията е прекратена.{reset}");
                    return;
            }


            //добавяне на цената на стаята
            Console.Write($"{reset}Моля въведете цената на стаята за нощувка (XX.XX): {pink}");
            decimal pricePerNight;
            try //проверка дали input-а е валиден
            {
                pricePerNight = decimal.Parse(Console.ReadLine());
                if (pricePerNight <= 0) //проверка дали цената е положително число
                {
                    Console.Clear();
                    Console.WriteLine($"{red}Цената на стаята трябва да бъде положително число. Операцията е прекратена.{reset}");
                    return;
                }
                pricePerNight = Math.Round(pricePerNight, 2); //закръглане до 2 знака след десетичната запетая (не работи напълно)
            }
            catch
            {
                Console.Clear();
                Console.WriteLine($"{red}Невалидна цена на стаята. Операцията е прекратена.{reset}");
                return;
            }

            //добавяне на статуса на стаята
            Console.Write($"{reset}Моля въведете дали стаята да е заета (да/не): {pink}");
            bool isOccupied;
            string guestName;
            switch (Console.ReadLine().ToLower())
            {
                //в случай че се използва друга клавиатура
                case "да":
                case "da":
                case "yes":
                    isOccupied = true;
                    //добавяне на името на госта
                    Console.Write($"{reset}Моля въведете името на госта, който ще заеме стаята: {pink}");
                    guestName = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Името на госта не може да бъде празно. Операцията е прекратена.{reset}");
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
                    isOccupied = false;
                    guestName = null;
                    Console.Clear();
                    Console.WriteLine($"{red}Невалиден отговор. Стаята ще бъде добавена като свободна.{reset}");
                    return;
            }
            //добавяне на новата стая
            Room newRoom = new Room(roomNumber, type, capacity, pricePerNight, isOccupied, guestName);
            data.Rooms.Add(newRoom);
            data.Save();

            Console.Clear();
            Console.WriteLine($"{green}Стая номер {pink}{newRoom.RoomNumber}{green} е успешно добавена.{reset}");
        }

        //----------------------Edit room------------------------------------

        public void EditRoom()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}---------------Редактиране на стая----------------{reset}");
            Console.WriteLine($"{cyan}Списък със стаи:{reset}");
            DisplayAllRooms();
            Console.WriteLine($"{cyan}--------------------------------------------------{reset}");
            Console.Write($"{reset}Въведете номера на стаята, която желаете да редактирате: {pink}");

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
                    Console.WriteLine($"{red}Стая с този номер не съществува. Операцията е прекратена.{reset}");
                    return;
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine($"{red}Невалиден номер на стая. Операцията е прекратена.{reset}");
                return;
            }

            Console.WriteLine($"{cyan}Какво искате да редактирате?{reset}");
            Console.WriteLine("1. Номера на стаята");
            Console.WriteLine("2. Типа на стаята");
            Console.WriteLine("3. Цената на стаята за нощувка");
            Console.WriteLine("4. Статуса на стаята");
            Console.WriteLine("5. Името на госта");
            Console.WriteLine("6. Изход от редактирането на стаята");
            Console.WriteLine($"{cyan}--------------------------------------------------{reset}");
            Console.Write($"{reset}Моля въведете вашия избор: {pink}");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": //Edit room number
                    Console.Write($"{reset}Моля въведете новия номер на стаята: {pink}");
                    try //проверка дали input-а е валиден
                    {
                        int newRoomNumber = int.Parse(Console.ReadLine());
                        if (data.Rooms.Any(r => r.RoomNumber == newRoomNumber)) //проверка дали новия номер на стаята вече съществува
                        {
                            Console.Clear();
                            Console.WriteLine($"{red}Стая с този номер вече съществува. Операцията е прекратена.{reset}");
                            return;
                        }
                        roomToEdit.RoomNumber = newRoomNumber;
                        Console.Clear();
                        Console.WriteLine($"{green}Номера на стаята е успешно променен на {pink}{newRoomNumber}{green}.{reset}");
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Невалиден номер на стая. Операцията е прекратена.{reset}");
                        return;
                    }
                    break;
                case "2": //Edit room type
                    Console.WriteLine($"{cyan}Видове стаи:{reset}");
                    Console.WriteLine("1. Единична");
                    Console.WriteLine("2. Двойна");
                    Console.WriteLine("3. Луксозна");
                    Console.WriteLine("4. Апартамент");
                    Console.WriteLine("5. Фамилна");
                    Console.WriteLine("6. Друг");
                    Console.WriteLine($"{cyan}--------------------------------------------------{reset}");
                    Console.Write($"{reset}Моля въведете новия тип на стаята: {pink}");
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
                            Console.Write($"{reset}Моля въведете вашия тип на стаята: {pink}");
                            //написване на вида с главна буква
                            newType = Console.ReadLine()?.ToLower();

                            if (string.IsNullOrEmpty(newType) || newType.All(char.IsDigit)) //проверка дали input-а е валиден
                            {
                                Console.Clear();
                                Console.WriteLine($"{red}Типът на стаята не може да бъде празен или да съдържа число. Операцията е прекратена.{reset}");
                                return;
                            }
                            newType = char.ToUpper(newType[0]) + newType.Substring(1);

                            Console.Write($"{reset}Моля въведете новия капацитет на стаята: {pink}");
                            try //проверка дали input-а е валиден
                            {
                                newCapacity = int.Parse(Console.ReadLine());
                                if (newCapacity <= 0) //проверка дали капацитета е положително число
                                {
                                    Console.Clear();
                                    Console.WriteLine($"{red}Капацитетът на стаята трябва да бъде положително число. Операцията е прекратена.{reset}");
                                    return;
                                }
                                roomToEdit.Capacity = newCapacity;
                                data.Save();

                                Console.Clear();
                                Console.WriteLine($"{green}Капацитетът на стаята е успешно променен на {pink}{newCapacity}{green}.{reset}");
                            }
                            catch
                            {
                                Console.Clear();
                                Console.WriteLine($"{red}Невалиден капацитет на стаята. Операцията е прекратена.{reset}");
                                return;
                            }
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine($"{red}Невалиден тип на стаята. Операцията е прекратена.{reset}");
                            return;
                    }
                    roomToEdit.Type = newType;
                    data.Save();

                    Console.Clear();
                    Console.WriteLine($"{green}Типът на стаята е успешно променен на {pink}{newType}{green}.{reset}");
                    break;
                case "3": //Edit room price
                    Console.Write($"{reset}Моля въведете новата цена на стаята за нощувка (XX.XX): {pink}");
                    decimal newPricePerNight;
                    try //проверка дали input-а е валиден
                    {
                        newPricePerNight = decimal.Parse(Console.ReadLine());
                        if (newPricePerNight <= 0) //проверка дали цената е положително число
                        {
                            Console.Clear();
                            Console.WriteLine($"{red}Цената на стаята трябва да бъде положително число. Операцията е прекратена.{reset}");
                            return;
                        }
                        newPricePerNight = Math.Round(newPricePerNight, 2); //закръглане до 2 знака след десетичната запетая (не работи напълно)
                        roomToEdit.PricePerNight = newPricePerNight;
                        data.Save();

                        Console.Clear();
                        Console.WriteLine($"{green}Цената на стаята е успешно променена на {pink}{newPricePerNight}{green} лв. на нощувка.{reset}");
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Невалидна цена на стаята. Операцията е прекратена.{reset}");
                        return;
                    }
                    break;
                case "4": //Edit room status
                    if (roomToEdit.Occupied) //проверка дали сатята е заета
                    {
                        roomToEdit.Occupied = false;
                        roomToEdit.GuestName = null;
                        Console.Clear();
                        Console.WriteLine($"{green}Стая номер {pink}{roomToEdit.RoomNumber}{green} е успешно освободена.{red}");
                    }
                    else
                    {
                        roomToEdit.Occupied = true;
                        Console.Write($"{reset}Моля въведете името на госта, който ще заеме стаята: {pink}");
                        //написване на името с главна буква
                        string guestName = Console.ReadLine()?.ToLower();

                        if (string.IsNullOrEmpty(guestName)) //проверка дали името на госта е празно
                        {
                            Console.Clear();
                            Console.WriteLine($"{red}Името на госта не може да бъде празно. Операцията е прекратена.{reset}");
                            return;
                        }
                        guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);
                        roomToEdit.GuestName = guestName; //промяна на името на госта ако стаята се резервира

                        Console.Clear();
                        Console.WriteLine($"{green}Стая номер {pink}{roomToEdit.RoomNumber}{green} е успешно резервирана за {pink}{guestName}{green}.{reset}");
                    }
                    data.Save();
                    break;

                case "5": //Edit guest name
                    if (roomToEdit.Occupied) //проверка дали сатята е заета
                    {
                        Console.Write($"{reset}Моля въведете новото име на госта: {pink}");
                        //написване на името с главна буква
                        string newGuestName = Console.ReadLine()?.ToLower();

                        if (string.IsNullOrEmpty(newGuestName)) //проверка дали името на госта е празно
                        {
                            Console.Clear();
                            Console.WriteLine($"{red}Името на госта не може да бъде празно. Операцията е прекратена.{reset}");
                            return;
                        }
                        newGuestName = char.ToUpper(newGuestName[0]) + newGuestName.Substring(1);
                        roomToEdit.GuestName = newGuestName; //промяна на името на госта

                        Console.Clear();
                        Console.WriteLine($"{green}Името на госта е успешно променено на {pink}{newGuestName}{green}.{reset}");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{red}Стаята не е заета. Няма нужда от промяна на името на госта.{reset}");
                    }
                    data.Save();
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine($"{green}Изход от редактирането на стаята.{reset}");
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine($"{red}Невалиден избор. Операцията е прекратена.{reset}");
                    return;
            }
        }

        //----------------------Remove room------------------------------------

        //премахване на стая
        public void RemoveRoom()
        {
            Console.Clear();
            Console.WriteLine($"\n{cyan}----------------Премахване на стая----------------{reset}");
            Console.WriteLine($"{cyan}Списък със стаи:{reset}");
            DisplayAllRooms();
            Console.WriteLine($"{cyan}--------------------------------------------------{reset}");
            Console.Write($"{reset}Въведете номера на стаята, която желаете да премахнете: {pink}");
            int roomNumber;
            try //проверка дали input-а е валиден
            {
                roomNumber = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.Clear();
                Console.WriteLine($"{red}Невалиден номер на стая. Операцията е прекратена.{reset}");
                return;
            }

            if (!data.Rooms.Any(r => r.RoomNumber == roomNumber)) //проверка дали стаята с този номер съществува
            {
                Console.Clear();
                Console.WriteLine($"{red}Стая с този номер не съществува. Операцията е прекратена.{reset}");
                return;
            }
            foreach (Room room in data.Rooms)
            {
                if (room.RoomNumber == roomNumber) //намиране на стаята по номер
                {
                    data.Rooms.Remove(room);
                    data.Save();

                    Console.Clear();
                    Console.WriteLine($"{green}Стая номер {pink}{room.RoomNumber}{green} е успешно премахната.{reset}");
                    return;
                }
            }
        }

        //----------------------Return to default rooms------------------------------------

        //възстановяване на списъка със стаите по умолчание
        public void ResetToDefaultRooms()
        {
            Console.Write($"{reset}Сигурни ли сте, че желаете да възстановите списъка със стаите (да/не): {pink}");
            string confirmation = Console.ReadLine()?.ToLower();
            switch (confirmation)
            {
                //в случай че се използва друга клавиатура
                case "да":
                case "da":
                case "yes":
                    data.ResetRoomsToDefault();
                    Console.Clear();
                    Console.WriteLine($"{green}Възстановяване на списъка...{reset}");
                    break;
                //в случай че се използва друга клавиатура
                case "не":
                case "ne":
                case "no":
                    Console.Clear();
                    Console.WriteLine($"{green}Възстановяването е отменено.{reset}");
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine($"{red}Невалиден отговор. Вие ще бъдете върнати в админ панела.{reset}");
                    break;
            }
        }
    }
}
