using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

            My_Lib start_session = new My_Lib();
            start_session.Starter(dir, NN_path);
        }
    }
}
