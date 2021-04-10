using System;
using System.Threading;

namespace ConsoleThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Primary Thread stats *****\n");
            // Получить имя текущего потока.
            Thread primaryThread = Thread.CurrentThread;
            primaryThread.Name = "ThePrimaryThread";
            // Показать детали обслуживающего домена приложения и контекста.
            Console.WriteLine("Name of current AppDomain: {0}",
            Thread.GetDomain().FriendlyName); // Имя текущего домена приложения
            //Console.WriteLine("ID of current Context: {0}",
            //Thread.CurrentContext.ContextID); // Идентификатор текущего контекста
                                              // Вывести некоторую статистику о текущем потоке.
            Console.WriteLine("Thread Name: {0}", primaryThread.Name); //Имя потока
            Console.WriteLine("Has thread started?: {0}",
            primaryThread.IsAlive,primaryThread.Name); // Запущен ли поток 
            Console.WriteLine("Priority Level: {0}",
            primaryThread.Priority); // Приоритет потока
            Console.WriteLine("Thread State: {0}",
            primaryThread.ThreadState); // Состояние потока
            Console.ReadLine();
        }
    }
}
