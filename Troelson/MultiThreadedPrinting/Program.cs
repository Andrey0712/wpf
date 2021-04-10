using System;
using System.Threading;


namespace MultiThreadedPrinting
{
   
    public class Printer 
    {
        private object threadLock = new object();
        public void PrintNumbers()
        {
            lock (threadLock)
            {
            Console.WriteLine("-> { 0} is executing PrintNumbers ()");
                // Вывести числа.
                Console.Write("Your numbers: ");
            for (int i = 0; i < 10; i++)
            {
                Random r = new Random();
                Thread.Sleep(100 * r.Next(5));
                Console.Write("{0}, ", i);
                
            }
            
            }
              Console.WriteLine();  // Вывести информацию о потоке.
                
        }
    }

    class Program
    {
        static void PrintTime(object state)
        {
            //Console.WriteLine("Time is: { 0} ",DateTime.Now.ToLongTimeString() ) ;
            Console.WriteLine("Time is: {0}, Param is: {1}",DateTime.Now.ToLongTimeString(), state.ToString());
        }
        static void Main(string[] args)
        {
            //Console.WriteLine("*****Synchronizing Threads *****\n");
            //Printer p = new Printer();
            //// Создать 10 потоков, которые указывают на один
            ////и тот же метод того же самого объекта.
            //Thread[] threads = new Thread[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    threads[i] = new Thread(new ThreadStart(p.PrintNumbers))
            //    {
            //        Name = $"Worker thread #{i}"
            //    };
            //    // Теперь запустить их все.
            //    foreach (Thread t in threads)
            //    {
            //       t.Start();
            //    };
                    
            //    Console.ReadLine();
            //}

            //Console.WriteLine("***** Working with Timer type *****\n");
            //// Создать делегат для типа Timer.
            //TimerCallback timeCB = new TimerCallback(PrintTime);
            //Timer t = new Timer(timeCB, "Hello From Main", 0, 1000);
            //// Установить параметры таймера.
            ////Timer t = new Timer(
            ////timeCB, // Объект делегата TimerCallback.
            ////null, // Информация для передачи в вызванный метод
            ////      // (null, если информация отсутствует).
            ////0, // Период ожидания перед запуском (в миллисекундах)
            ////1000); // Интервал между вызовами (в миллисекундах).
            //Console.WriteLine("Hit key to terminate...");
            //Console.ReadLine();

            Console.WriteLine("***** Fun with the CLR Thread Pool *****\n");
            Console.WriteLine("Main thread started. ThreadID = {0}",
            Thread.CurrentThread.ManagedThreadId);
            Printer p = new Printer();
            WaitCallback workltem = new WaitCallback(PrintTheNumbers);
            // Поставить в очередь метод десять раз.
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(workltem, p);
            }
            Console.WriteLine("All tasks queued");
            Console.ReadLine();
        }
        static void PrintTheNumbers(object state)
        {
            Printer task = (Printer)state;
            task.PrintNumbers();
        }
    }
}
