using System;
using System.Threading;

namespace AddWithThreads
{
    public class AddParams
    {
        public int a, b;
        public AddParams(int numb1, int numb2)
        {
           a = numb1;
           b = numb2;
        }
          
    }

    class Program
    {
        private static AutoResetEvent waitHandle = new AutoResetEvent(false);
        static void Add(object data)
        {
            if (data is AddParams)
            {
                Console.WriteLine("ID of thread in Add(): {0}", Thread.CurrentThread.ManagedThreadId);
                AddParams ap = (AddParams)data;
                Console.WriteLine("{0} + {1} is {2}", ap.a, ap.b, ap.a + ap.b);
                // Сообщить другому потоку о том, что работа завершена.
                waitHandle.Set();
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("***** Adding with Thread objects *****");
            Console.WriteLine("ID of thread in MainO : {0}", Thread.CurrentThread.ManagedThreadId);
            // Создать объект AddParams для передачи вторичному потоку.
            AddParams ар = new AddParams(10, 10);
            Thread t = new Thread(new ParameterizedThreadStart(Add));
            t.Start(ар);
            //// Подождать, пока другой поток завершится.
            //Thread.Sleep(5);
            //Console.ReadLine();
            // Ожидать, пока не поступит уведомление!
            waitHandle.WaitOne();
            Console.WriteLine("Other thread is done!");
            Console.ReadLine();

            
        }
    }
}
