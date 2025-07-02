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
            Console.WriteLine("x. Изход от програмата");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете желаемата от вас услуга: ");
        }

        public void ReservateRoom()
        {
            Console.WriteLine();
            Console.WriteLine("----------------Резервиране на стая---------------");
            Console.WriteLine("Списък със свободни стаи:");
            DisplayAvaibleRoom();
            Console.WriteLine("--------------------------------------------------");
            Console.Write("Моля въведете името на госта, който резервира стаята: ");
            string guestName = Console.ReadLine();
            Console.Write("Моля въведете номера на стаята, която искате да резервирате: ");
            int roomNumber = int.Parse(Console.ReadLine());
            Room roomToReserve = data.Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber && !r.Occupied);
            if (roomToReserve != null)
            {
                roomToReserve.Occupied = true;
                roomToReserve.GuestName = guestName;
                Console.WriteLine($"Стая номер {roomToReserve.RoomNumber} е успешно резервирана за {guestName}.");
            }
            else
            {
                Console.WriteLine("Стая с този номер не е намерена или вече е заета.");
            }
            data.Save();
            Console.WriteLine();
        }

        public void LeaveRoom()
        {
            Console.WriteLine();
            Console.WriteLine("---------------Освобождаване на стая--------------");
            Console.WriteLine("Списък със заети стаи:");
            DisplayOccupiedRoom();
            Console.Write("Моля въведете името на госта, който освобождава стаята: ");
            string guestName = Console.ReadLine();
            Room roomToLeave = data.Rooms.FirstOrDefault(r => r.GuestName == guestName && r.Occupied);
            if (roomToLeave != null)
            {
                roomToLeave.Occupied = false;
                roomToLeave.GuestName = null;
                Console.WriteLine($"Стая номер {roomToLeave.RoomNumber} е успешно освободена.");
            }
            else
            {
                Console.WriteLine("Няма намерени стаи за освобождаване с това име.");
            }
            data.Save();
            Console.WriteLine();
        }

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
    }
}
