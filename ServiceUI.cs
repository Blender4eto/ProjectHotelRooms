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
            Console.WriteLine("-----------------Хотел Черноморец-----------------");
            Console.WriteLine("Списък с предлагани услуги:");
            Console.WriteLine("1. Резервиране на стая");
            Console.WriteLine("2. Освобождаване на стая");
            Console.WriteLine("3. Проверка на наличността и цените на стаите");
            Console.WriteLine("4. Справка за заетите стаи и техните гости");
            Console.WriteLine("5. Админ панел");
            Console.WriteLine("x. Изход от програмата");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете желаемата от вас услуга: ");
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
                Console.Write("Моля въведете името на госта, който резервира стаята: ");
                string guestName = Console.ReadLine().ToLower();
                Console.Write("Моля въведете номера на стаята, която искате да резервирате: ");
                int roomNumber = int.Parse(Console.ReadLine());
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
            if (occupiedRooms.Count != 0)
            {
                Console.Write("Моля въведете името на госта, който освобождава стаята: ");
                string guestName = Console.ReadLine().ToLower();
                Room roomToLeave = null;
                foreach (Room room in data.Rooms)
                {
                    if (room.GuestName == guestName && room.Occupied)
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
                    Console.WriteLine("Няма намерени стаи за освобождаване с гост с това име.");
                }
                data.Save();
            }

            Console.WriteLine();
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


        public void AdminPanel()
        {
            Console.WriteLine();
            Console.WriteLine("---------------Админ панел----------------");
            Console.WriteLine("Тук ще бъдат добавени админ функции.");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine();
            DisplayMenu();
        }
    }
}
