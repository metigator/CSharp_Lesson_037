using System;
using System.Threading;

namespace CADeadLock
{
    class Program
    {
        static void Main(string[] args)
        {
            var wallet1 = new Wallet(1, "Issam", 100);
            var wallet2 = new Wallet(2, "Reem", 50);
            Console.WriteLine("\n Before Transaction");
            Console.WriteLine("\n---------------------");
            Console.Write(wallet1 + ", "); Console.Write(wallet2); Console.WriteLine();
            Console.WriteLine("\n After Transaction");
            Console.WriteLine("\n---------------------");

            var transferManager1 = new TransferManager(wallet1, wallet2, 50);
            var transferManager2 = new TransferManager(wallet2, wallet1, 30);

            var t1 = new Thread(transferManager1.Transfer);
            t1.Name = "T1";
            var t2 = new Thread(transferManager2.Transfer);
            t2.Name = "T2";

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
            
            Console.Write(wallet1 + ", "); Console.Write(wallet2); Console.WriteLine();
            Console.ReadKey();
        }
    }

    class Wallet
    {
        private readonly object bitcoinsLock = new object();
        public Wallet(int id, string name, int bitcoins)
        {
            Id = id;
            Name = name;
            Bitcoins = bitcoins;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Bitcoins { get; private set; }


        public void Debit(int amount)
        {
            lock (bitcoinsLock)
            {
                if (Bitcoins >= amount)
                {
                    Thread.Sleep(1000);

                    Bitcoins -= amount;
                }
            }
        }

        public void Credit(int amount)
        {
            Thread.Sleep(1000);
            Bitcoins += amount;
        }


        public override string ToString()
        {
            return $"[{Name} -> {Bitcoins} Bitcoins]";
        }
    }

    class TransferManager
    {
        private Wallet from;
        private Wallet to;

        private int amountToTransfer;

        public TransferManager(Wallet from, Wallet to, int amountToTransfer)
        {
            this.from = from;
            this.to = to;
            this.amountToTransfer = amountToTransfer;
        }

        public void Transfer()
        {
            var lock1 = from.Id < to.Id ? from : to;
            var lock2 = from.Id < to.Id ? to : from;

            Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock ... {from}");
            lock (lock1)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} lock acquired ... {from}");
                Thread.Sleep(1000);
                Console.WriteLine($"{Thread.CurrentThread.Name} trying to lock ... {to}");

                lock (lock2)
                {
                    from.Debit(amountToTransfer);
                    to.Credit(amountToTransfer);
                }
            }
        }
    }
}
