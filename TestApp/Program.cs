using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestFunktion();
        }
        static void TestFunktion()
        {
            Console.WriteLine("Tue Etwas und warte auf Benutzereingabe...");
            Console.ReadKey();
        }
    }
}
