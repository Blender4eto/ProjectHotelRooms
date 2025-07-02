namespace ProjectHotelRooms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Data data = new Data(); // Loads rooms from JSON
            data.DisplayRooms();    // Displays rooms to CMD
            data.ReservateRoom();
         
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }
    }
}
