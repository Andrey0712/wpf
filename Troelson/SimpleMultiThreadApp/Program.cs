using System;
using System.Threading;

namespace SimpleMultiThreadApp
{
    public class Printer
    {
        public void PrintNumbers()
        {
            // Вывести информацию о потоке.
            Console.WriteLine("-> { 0} is executing PrintNumbers ()");
            Console.Write("Your numbers: ");
for (int i = 0; i < 10; i ++)
            {
                Console.Write("{0}, ", i);
                Thread.Sleep(500);
            }
            Console.WriteLine(); 
        } 
    }
// Вывести числа.
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** The Amazing Thread App *****\n");
            Console.Write("Do you want [1] or [2] threads? ");
            string threadCount = Console.ReadLine(); // Запрос количества потоков
                                                     // Назначить имя текущему потоку.
            Thread pimaryThread = Thread.CurrentThread;
            pimaryThread.Name = "Primary";
            // Вывести информацию о потоке.
            Console.WriteLine("-> {0} is executing Main ()",
            Thread.CurrentThread.Name);
            // Создать рабочий класс.
            Printer p = new Printer();
            Thread bgroundThread =
new Thread(new ThreadStart(p.PrintNumbers));
            // Теперь это фоновый поток.
            bgroundThread.IsBackground = true;
            bgroundThread.Start();
            switch (threadCount)
            {
                case "2":
                    // Создать поток.
                    Thread backgroundThread =
                    new Thread(new ThreadStart(p.PrintNumbers));
                    backgroundThread.Name = "Secondary";
                    backgroundThread.Start();
                    break;
                case "1":
                    p.PrintNumbers();
                    break;
                default:
                    Console.WriteLine("I don't know what you want...you get 1 thread.");
                    goto case "1"; // Переход к варианту с одним потоком
            }
            // Выполнить некоторую дополнительную работу.
            Console.Write("I'm busy!", "Work on main thread...");
            Console.ReadLine();
        }
    }
    
}
