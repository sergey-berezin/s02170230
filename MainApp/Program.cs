using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NN_lib;

namespace Image_recognition
{
    class Program
    {
        static void Pred(PredictionResult sender, EventArgs e)
        {
            if (sender != null)
            {

                Console.WriteLine($"file: {sender.Path} result: {sender.ClassLabel}");

            }
        }

        static void Main(string[] args)
        {
            string dirpath, modelpath;

            Console.WriteLine("Please enter images directory");
            dirpath = Console.ReadLine();

            Console.WriteLine("Please enter model directory");
            modelpath = Console.ReadLine();

            Class1 recon = new Class1(modelpath);
            recon.Notify += Pred;
            Thread cancelTread = new Thread(() =>
            {
                while (true)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C)
                    {
                        recon.Stop();
                        break;
                    }
                    if (Class1.endSignal == Directory.GetFiles(dirpath, "*.jpg").Length)
                        break;
                }
            }
            );
            cancelTread.Start();
            recon.ParallelProcess(dirpath);


        }

    }
}