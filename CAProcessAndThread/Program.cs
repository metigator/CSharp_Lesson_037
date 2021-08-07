using System;
using System.Diagnostics;
using System.Threading;

namespace CAProcessAndThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Process Id: {Process.GetCurrentProcess().Id}");
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Processor Id: {Thread.GetCurrentProcessorId()}");
            Console.ReadKey();
        }
    }
}
