using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectHotelRooms
{
    public class ServiceUI
    {
        private static Data data = new Data();

        public void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----------------Хотел Черноморец-----------------");
            Console.WriteLine("Списък с предлагани услуги:");
            Console.WriteLine("1. Резервиране на стая");
            Console.WriteLine("2. Освобождаване на стая");
            Console.WriteLine("3. Проверка на наличността и цените на стаите");
            Console.WriteLine("4. Справка за заетите стаи и техните гости");
            Console.WriteLine("5. Админ панел");
            Console.WriteLine("x. Изход от програмата");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете вашия избор: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //----------------------Reservate room------------------------------------

        public void ReservateRoom()
        {
            Console.WriteLine();
            Console.WriteLine("----------------Резервиране на стая---------------");
            Console.WriteLine("Списък със свободни стаи:");
            DisplayAvaibleRoom();
            Console.WriteLine("--------------------------------------------------");

            List<Room> availableRooms = data.DisplayAvaibleRooms();

            if (availableRooms.Count != 0)
            {
                Console.Write("Моля въведете номера на стаята, която искате да резервирате: ");
                int roomNumber;
                try
                {
                    roomNumber = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Невалиден номер. Операцията е прекратена.");
                    Console.WriteLine();
                    return;
                }
                Console.Write("Моля въведете името на госта, който резервира стаята: ");
                string guestName = Console.ReadLine().ToLower();
                guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

                Room roomToReservate = null;

                foreach (Room room in data.Rooms)
                {
                    if (room.RoomNumber == roomNumber && !room.Occupied)
                    {
                        roomToReservate = room;
                        break;
                    }
                }

                if (roomToReservate != null)
                {
                    roomToReservate.Occupied = true;
                    roomToReservate.GuestName = guestName;
                    Console.WriteLine($"Стая номер {roomToReservate.RoomNumber} е успешно резервирана за {guestName}.");
                }
                else
                {
                    Console.WriteLine("Стая с този номер не е намерена или вече е заета.");
                }
                data.Save();
            }
            Console.WriteLine();
        }

        //----------------------Leave room------------------------------------

        public void LeaveRoom()
        {
            Console.WriteLine();
            Console.WriteLine("---------------Освобождаване на стая--------------");
            Console.WriteLine("Списък със заети стаи:");
            DisplayOccupiedRoom();
            Console.WriteLine("--------------------------------------------------");

            List<Room> occupiedRooms = data.DisplayOccupiedRooms();
            List<Room> occupiedRoomsByPerson = new List<Room>();

            if (occupiedRooms.Count != 0)
            {
                // TODO: maybe add a option to leave several rooms at once with ', ' or ',' seperator or leave all at once
                Console.Write("Моля въведете името на госта, който освобождава стаята: ");
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

                if (occupiedRoomsByPerson != null && occupiedRoomsByPerson.Count > 1)
                {
                    Console.WriteLine($"Гостът {guestName} има {occupiedRoomsByPerson.Count} резервации.");
                    Console.WriteLine("Списък с резервациите на госта:");
                    foreach (var room in occupiedRoomsByPerson)
                    {
                        Console.WriteLine($"| Стая номер {room.RoomNumber}, Вид: {room.Type}, Гост: {room.GuestName}");
                    }
                    Console.WriteLine("--------------------------------------------------");
                    Console.Write("Въведете коя от резервациите на госта да се освободи: ");

                    int roomNumber;
                    try
                    { 
                        roomNumber = int.Parse(Console.ReadLine()); 
                    }
                    catch 
                    {
                        Console.WriteLine("Невалиден номер на стая. Моля, опитайте отново.");
                        Console.WriteLine();
                        return;
                    }

                    foreach (Room room in data.Rooms)
                    {
                        if (room.RoomNumber == roomNumber && room.Occupied)
                        {
                            roomToLeave = room;
                            break;
                        }
                    }

                    if (roomToLeave != null)
                    {
                        roomToLeave.Occupied = false;
                        roomToLeave.GuestName = null;
                        Console.WriteLine($"Стая номер {roomToLeave.RoomNumber} е успешно освободена.");
                    }
                    else
                    {
                        Console.WriteLine("Стая с този номер не е намерена или вече е свободна.");
                    }
                }
                else if (occupiedRoomsByPerson != null && occupiedRoomsByPerson.Count == 1)
                {
                    roomToLeave = occupiedRoomsByPerson[0];
                    roomToLeave.Occupied = false;
                    roomToLeave.GuestName = null;
                    Console.WriteLine($"Стая номер {roomToLeave.RoomNumber} е успешно освободена.");
                }
                else
                {
                    Console.WriteLine("Няма намерени стаи за освобождаване с гост с това име.");
                }
                data.Save();
            }
            Console.WriteLine();
        }

        //----------------------All rooms------------------------------------

        public void DisplayAllRoomsUI()
        {
            Console.WriteLine();
            Console.WriteLine("--------------Списък със всички стаи--------------");
            DisplayAllRooms();
            Console.WriteLine();
        }

        public void DisplayAllRooms()
        {
            List<Room> allRooms = data.DisplayAllRooms();
            if (allRooms.Count == 0)
            {
                Console.WriteLine("Няма налични стаи.");
            }
            else
            {
                foreach (var room in allRooms)
                {
                    if(room.Occupied)
                        Console.WriteLine($"| Стая номер {room.RoomNumber}, Вид: {room.Type}, Цена: {room.PricePerNight} лв. на нощувка, Статус: Заета, Гост: {room.GuestName}");
                    else
                    {
                        Console.WriteLine($"| Стая номер {room.RoomNumber}, Вид: {room.Type}, Цена: {room.PricePerNight} лв. на нощувка, Статус: Свободна, Гост: Няма");
                    }
                }
            }
        }

        //----------------------Avaible rooms------------------------------------

        public void DisplayAvaibleRoomUI()
        {
            Console.WriteLine();
            Console.WriteLine("--------------Списък със свободни стаи------------");
            DisplayAvaibleRoom();
            Console.WriteLine();
        }

        public void DisplayAvaibleRoom()
        {
            List<Room> availableRooms = data.DisplayAvaibleRooms();

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

        public void DisplayOccupiedRoomUI()
        {
            Console.WriteLine();
            Console.WriteLine("---------------Списък със заети стаи--------------");
            DisplayOccupiedRoom();
            Console.WriteLine();
        }

        public void DisplayOccupiedRoom()
        {
            List<Room> occupiedRooms = data.DisplayOccupiedRooms();

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

        public const string AdminPassword = "salam";

        public bool EnterAdminPanel()
        {   
            Console.WriteLine();
            Console.WriteLine("-------------------Админ панел--------------------");
            Console.Write("Въведете парола: ");
            if (Console.ReadLine() == AdminPassword)
            {
                DisplayAdminMenu();
                return true;
            }
            else
            {
                Console.WriteLine("Невалидна парола, вие ще бъдете върнати в главното меню.");
                Console.WriteLine();
                return false;
            }
        }

        public void DisplayAdminMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("------------Добре дошли в админ панела------------");
            Console.WriteLine("1. Справка за всички стаи");
            Console.WriteLine("2. Резервиране на всички стаи");
            Console.WriteLine("3. Освобождаване на всички стаи");
            Console.WriteLine("4. Добавяне на стая");
            Console.WriteLine("5. Редактиране на стая");
            Console.WriteLine("6. Премахване на стая");
            Console.WriteLine("7. Връщане на списъка на стаите по умолчание");
            Console.WriteLine("8. Изход от админ панела");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете вашия избор: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //----------------------Reservate all rooms------------------------------------

        public void ReservateAllRooms()
        {
            Console.WriteLine();
            Console.WriteLine("-------Резервиране на всички свободни стаи--------");

            List<Room> availableRooms = data.DisplayAvaibleRooms();
            string guestName;

            if (availableRooms.Count != 0)
            {
                Console.Write("Моля въведете името на госта, който резервира свободните стаи: ");
                guestName = Console.ReadLine().ToLower();

                if (string.IsNullOrEmpty(guestName))
                {
                    Console.WriteLine("Името на госта не може да бъде празно. Операцията е прекратена.");
                    return;
                }
                guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

                foreach (var room in data.Rooms)
                {
                    if (!room.Occupied)
                    {
                        room.Occupied = true;
                        room.GuestName = guestName;
                    }
                }
                Console.WriteLine($"Всички свободни стаи са резервирани от {guestName}.");
                data.Save();
            }
            else
            {
                Console.WriteLine("Няма свободни стаи за резервиране.");
                return;
            }
        }

        //----------------------Leave all rooms------------------------------------

        public void LeaveAllRooms()
        {
            // TODO: add a option to leave selected rooms + option to kick out guest and leave all his rooms
            Console.WriteLine();
            Console.WriteLine("-----------Освобождаване на всички стаи-----------");

            foreach (var room in data.Rooms)
            {
                room.Occupied = false;
                room.GuestName = null;
            }
            Console.WriteLine("Всички стаи са освободени.");
            data.Save();
        }

        //----------------------Add room------------------------------------

        public void AddRoom()
        {
            Console.WriteLine();
            Console.WriteLine("--------------Създаване на нова стая--------------");

            //Добавяне на номера на стаята
            Console.Write("Моля въведете номера на стаята: ");
            int roomNumber;
            try
            {
                roomNumber = int.Parse(Console.ReadLine());
                if (data.Rooms.Any(r => r.RoomNumber == roomNumber))
                {
                    Console.WriteLine("Стая с този номер вече съществува. Операцията е прекратена.");
                    return;
                }
            }
            catch
            {
                Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                return;
            }

            //Добавяне на типа на стаята
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
                    type = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(type) || type.All(char.IsDigit))
                    {
                        Console.WriteLine("Типът на стаята не може да бъде празен или да съдържа число. Операцията е прекратена.");
                        return;
                    }
                    type = char.ToUpper(type[0]) + type.Substring(1);

                    //Добавяне на капацитета на стаята
                    Console.Write("Моля въведете номер на капацитета на стаята: ");
                    try
                    {
                        capacity = int.Parse(Console.ReadLine());
                        if (capacity <= 0)
                        {
                            Console.WriteLine("Капацитетът на стаята трябва да бъде положително число. Операцията е прекратена.");
                            return;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Невалиден капацитет на стаята. Операцията е прекратена.");
                        Console.WriteLine();
                        return;
                    }
                    break;

                default:
                    Console.WriteLine("Невалиден тип на стаята. Операцията е прекратена.");
                    return;
            }


            //Добавяне на цената на стаята
            Console.Write("Моля въведете цената на стаята за нощувка (XX.XX): ");
            decimal pricePerNight;
            try
            {
                pricePerNight = decimal.Parse(Console.ReadLine());
                if (pricePerNight <= 0)
                {
                    Console.WriteLine("Цената на стаята трябва да бъде положително число. Операцията е прекратена.");
                    return;
                }
                pricePerNight = Math.Round(pricePerNight, 2); //not workin fully, maybe it will be deleted later
            }
            catch
            {
                Console.WriteLine("Невалидна цена на стаята. Операцията е прекратена.");
                Console.WriteLine();
                return;
            }

            //Добавяне на статуса на стаята
            Console.Write("Моля въведете дали стаята да е заета (да/не): ");
            bool isOccupied;
            string guestName;
            switch (Console.ReadLine()?.ToLower())
            {
                case "да":
                case "da":
                case "yes":
                    isOccupied = true;
                    //Добавяне на името на госта
                    Console.Write("Моля въведете името на госта, който ще заеме стаята: ");
                    guestName = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(guestName))
                    {
                        Console.WriteLine("Името на госта не може да бъде празно. Операцията е прекратена.");
                        return;
                    }
                    // TODO: maybe add something to prevent middle or last name to be with lower letter if there is one
                    guestName = char.ToUpper(guestName[0]) + guestName.Substring(1);

                    break;
                case "не":
                case "ne":
                case "no":
                    isOccupied = false;
                    guestName = null;
                    break;
                default:
                    Console.WriteLine("Невалиден отговор. Стаята ще бъде добавена като свободна.");
                    isOccupied = false;
                    guestName = null;
                    break;
            }

            //Добавяне на новата стая
            Room newRoom = new Room(roomNumber, type, capacity, pricePerNight, isOccupied, guestName);
            data.Rooms.Add(newRoom);
            Console.WriteLine($"Стая номер {newRoom.RoomNumber} е успешно добавена.");
            data.Save();
        }

        //----------------------Edit room------------------------------------

        public void EditRoom()
        {
            Console.WriteLine();
            Console.WriteLine("---------------Редактиране на стая----------------");
            Console.WriteLine("Списък със стаи:");
            DisplayAllRooms();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Въведете номера на стаята, която желаете да редактирате");

        }

        //----------------------Remove room------------------------------------

        public void RemoveRoom()
        {
            Console.WriteLine();
            Console.WriteLine("----------------Премахване на стая----------------");
            Console.WriteLine("Въведете номера на стаята, която желаете да премахнете");
            int roomNumber;
            try
            {
                roomNumber = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                return;
            }

            if (!data.Rooms.Any(r => r.RoomNumber == roomNumber))
            {
                Console.WriteLine("Стая с този номер не съществува. Операцията е прекратена.");
                return;
            }
            foreach (Room room in data.Rooms)
            {
                if (room.RoomNumber == roomNumber)
                {
                    data.Rooms.Remove(room);
                    Console.WriteLine($"Стая номер {room.RoomNumber} е успешно премахната.");
                    data.Save();
                    return;
                }
            }
        }

        //----------------------Return to default rooms------------------------------------

        public void ResetToDefaultRooms()
        {
            Console.Write("Сигурни ли сте, че желаете да възстановите списъка със стаите (да/не): ");
            string confirmation = Console.ReadLine()?.ToLower();
            switch (confirmation)
            {
                case "да":
                case "da":
                case "yes":
                    data.ResetRoomsToDefault();
                    Console.WriteLine("Възстановяване на списъка...");
                    break;
                case "не":
                case "ne":
                case "no":
                    Console.WriteLine("Възстановяването е отменено.");
                    break;
                default:
                    Console.WriteLine("Невалиден отговор. Вие ще бъдете върнати в админ панела.");
                    break;
            }
        }
    }
}
