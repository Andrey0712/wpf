using System;
using System.Threading;
using UserBudzet.Application.Interfaces;
using Budzet.Infrastructure.Services;

namespace ConsoleExamplThread
{
    class Program
    {
        static void Main(string[] args)
        {
            //int mainId = Thread.CurrentThread.ManagedThreadId;//id потока
            //Thread thread2 = new Thread(Thread2);//cоздаем поток2
            //Thread thread3 = new Thread(Thread3);
            //Console.WriteLine("Hello World!"+ mainId);
            //thread2.Start();//запускаем поток
            //thread3.Start();
            //Thread.Sleep(2000);//задержка запуска на 2 сек
            //Console.WriteLine("HW");

            Thread user_thread = new Thread(InsertUsers);//cоздаем поток
            user_thread.Start();


        }

        static void InsertUsers()
        {
 int count;
            IUserService userService = new UserService();
            Console.WriteLine("На старте в бд: " + userService.Count()+" юзеров");
            Console.WriteLine("Сколько народу добавит в базу: ");
            count = int.Parse(Console.ReadLine());
            
            userService.InsertUser(count);
            Console.WriteLine("Сейчас в бд: " + userService.Count() + " юзеров");
        }
        
        //static void Thread2()
        //{
        //    //Thread.Sleep(50);
        //    var colorDefalut = Console.ForegroundColor;
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    //Console.WriteLine("Thread "+threadId);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Green;
        //        Console.WriteLine($"{i}.Привіт земляни!" + threadId);
        //        Console.ForegroundColor = colorDefalut;
        //        Thread.Sleep(200);
        //    }
        //}

        //static void Thread3()
        //{
        //    var colorDefalut = Console.ForegroundColor;
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    //Console.WriteLine("Thread " + threadId);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine($"{i}.Привіт Колорадо! " + threadId);
        //        Thread.Sleep(200);
        //        Console.ForegroundColor = colorDefalut;
        //    }
        //}
    }
}
