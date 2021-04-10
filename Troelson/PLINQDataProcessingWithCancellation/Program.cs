using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQDataProcessingWithCancellation
{
    class Program
    {
        static CancellationTokenSource cancelToken = new CancellationTokenSource();
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Press any key to start processing");
                // Нажмите любую клавишу для начала обработки
                Console.ReadKey();
                Console.WriteLine("Processing");
                Task.Factory.StartNew(() => ProcessIntData());
                Console.Write("Enter Q to quit: ");
                // Введите Q для выхода:
                string answer = Console.ReadLine();
                // Желает ли пользователь выйти?
                if (answer.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    cancelToken.Cancel();
                    break;
                }
            } while (true);
            Console.ReadLine();
        }
        static void ProcessIntData()
        {
            // Получить очень большой массив целых чисел.
            int[] source = Enumerable.Range(1, 10000000).ToArray();
            // Найти числа, для которых истинно условие num % 3 == 0,и возвратить их в убывающем порядке.
            int[] modThreelsZero = null;
            try
            {
                modThreelsZero =
                (from num in source.AsParallel().WithCancellation(cancelToken.Token)
                 where num % 3 == 0
                 orderby num descending
                 select num).ToArray();
                Console.WriteLine();
                // Вывести количество найденных чисел.
                Console.WriteLine($"Found {modThreelsZero.Count()} numbers that match query!");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
