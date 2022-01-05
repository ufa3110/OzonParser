using System;

namespace OzonSearcher
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            new Parser.OzonParser("контейнеры+пластик").Open();
        }
    }
}
