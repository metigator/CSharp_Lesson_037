using System;
using System.Threading;

namespace CARaceCondition
{
    class Program
    {
        static void Main(string[] args)
        {
            var wallet = new Wallet("Issam", 50);

            //wallet.Debit(40);
            //wallet.Debit(30); // 10


            var t1 = new Thread(() => wallet.Debit(40));
            var t2 = new Thread(() => wallet.Debit(30));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine(wallet);
            Console.ReadKey();
        }
    }
    class Wallet
    {
        private readonly object bitcoinsLock = new object();
        public Wallet(string name, int bitcoins)
        {
            Name = name;
            Bitcoins = bitcoins;
        }

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
}
