using System;
using NN_lib;

namespace MainApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Enter path of your image collection");
            string dir = Console.ReadLine();
            Console.WriteLine("Enter neural network path");
            string NN_path = Console.ReadLine();

            My_Lib.ParallelProcess(dir, NN_path);
        }
    }
}
