using System;
using System.Threading;

namespace CAMultithreading
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main Thread";
            Console.WriteLine(Thread.CurrentThread.Name);
            //Console.WriteLine($"Background Thread: {Thread.CurrentThread.IsBackground}");
            var wallet = new Wallet("Issam", 80);

            Thread t1 = new Thread(wallet.RunRandomTransactions);
            t1.Name = "T1";
            //Console.WriteLine($"T1 Background Thread: {t1.IsBackground}");
            Console.WriteLine($"after declaration {t1.Name} state is: {t1.ThreadState}");

            t1.Start();
            Console.WriteLine($"after start() {t1.Name} state is: {t1.ThreadState}");
            t1.Join();

            Thread t2 = new Thread(new ThreadStart(wallet.RunRandomTransactions));
            t2.Name = "T2";
            t2.Start();

            Console.WriteLine($"after start {t1.Name} state is: {t1.ThreadState}");



            Console.ReadKey();
        }
    }

    class Wallet
    {
        public Wallet(string name, int bitcoins)
        {
            Name = name;
            Bitcoins = bitcoins;
        }

        public string Name { get; private set; }
        public int Bitcoins { get; private set; }


        public void Debit(int amount)
        {
            Thread.Sleep(1000);
            Bitcoins -= amount;
            Console.WriteLine(
                $"[Thread: {Thread.CurrentThread.ManagedThreadId}-{Thread.CurrentThread.Name} " +
                $", Processor Id: {Thread.GetCurrentProcessorId()}] -{amount}");
        }

        public void Credit(int amount)
        {
            Thread.Sleep(1000);
            Bitcoins += amount;
            Console.WriteLine(
                $"[Thread: {Thread.CurrentThread.ManagedThreadId}-{Thread.CurrentThread.Name} " +
                $", Processor Id: {Thread.GetCurrentProcessorId()}] +{amount}");
        }

        public void RunRandomTransactions()
        {
            int[] amounts = { 10, 20, 30, -20, 10, -10, 30, -10, 40, -20 }; // 80

            foreach (var amount in amounts)
            {
                var absValue = Math.Abs(amount);
                if (amount < 0)
                    Debit(absValue);
                else
                    Credit(absValue);

            }
        }

        public override string ToString()
        {
            return $"[{Name} -> {Bitcoins} Bitcoins]";
        }
    }
}
