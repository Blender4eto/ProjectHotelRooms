namespace ProjectHotelRooms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Data data = new Data(); // Loads rooms from JSON
            data.DisplayRoomsAll();    // Displays rooms to CMD
            data.ReservateRoom();
            data.LeaveRoom();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }
    }
}
