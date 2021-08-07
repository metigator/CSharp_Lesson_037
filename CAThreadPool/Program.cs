using System;
using System.Threading;
using System.Threading.Tasks;

namespace CAThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using ThreadPool");

            //1 
            ThreadPool.QueueUserWorkItem(new WaitCallback(Print));

            Console.WriteLine("Using Task");
            //2 
            Task.Run(Print);


            var employee = new Employee { Rate = 10, TotalHours = 40 };

            ThreadPool.QueueUserWorkItem(new WaitCallback(CalculateSalary), employee);

            Console.WriteLine(employee.TotalSalary);
            Console.ReadKey();
        }

        private static void CalculateSalary(object employee)
        {
            var emp = employee as Employee;
            if (employee is null)
                return;
            emp.TotalSalary = emp.TotalHours * emp.Rate;
            Console.WriteLine(emp.TotalSalary.ToString("C"));  
        }

        private static void Print()
        {
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId}, Thread Name: {Thread.CurrentThread.Name}");
            Console.WriteLine($"Is Pooled thread: {Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Cycle {i + 1}");
            }
        }

        private static void Print(object state)
        {
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId}, Thread Name: {Thread.CurrentThread.Name}");
            Console.WriteLine($"Is Pooled thread: {Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine($"Background: {Thread.CurrentThread.IsBackground}");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Cycle {i+1}");
            }
        }
    }

    class Employee
    {
        public decimal TotalHours { get; set; }
        public decimal Rate { get; set; }

        public decimal TotalSalary { get; set; }

    }

}
