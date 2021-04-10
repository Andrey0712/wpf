using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunWithCSharpAsync
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(" Fun With Async ===>");
            // Для подсказки Visual Studio модернизировать проект до версии C# 7.1.
            List<int> L = default;
            string message = await DoWorkAsync();
            Console.WriteLine(message);
            Console.WriteLine(DoWork());
            Console.WriteLine("Completed");

            await MethodReturningVoidAsync();
            Console.WriteLine("Void method complete");

            await MultiAwaits();

            Console.ReadLine();
        }
        static string DoWork()
        {
            Thread.Sleep(1_000);
            return "Done with work1!";
        }
        static async Task<string> DoWorkAsync()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(1_000);
                return "Done with work!";
            });
        }
        static async Task MethodReturningVoidAsync()
        {
            await Task.Run(() =>
            { /* Выполнить какую-то работу... */
                Thread.Sleep(4_000);
            });
            Console.WriteLine("Void method completed");
        }
        static async Task MultiAwaits()
        {
            await Task.Run(() => { Thread.Sleep(2_000); });
            Console.WriteLine("Done with first task!");
            await Task.Run(() => { Thread.Sleep(2_000); });
            Console.WriteLine("Done with second task!");
            await Task.Run(() => { Thread.Sleep(2_000); });
            Console.WriteLine("Done with third task!");
        }
        static async Task<string> MethodWithTryCatch()
        {
            try
            {
                // Выполнить некоторую работу.

                return "Hello";
            }
            catch (Exception ex)
            {
                await LogTheErrors();
                throw;
            }
            finally
            {
                await DoMagicCleanUp();
            }
        }

        private static Task LogTheErrors()
        {
            throw new NotImplementedException();
        }

        private static Task DoMagicCleanUp()
        {
            throw new NotImplementedException();
        }
        static async ValueTask<int> ReturnAnlnt()
        {
            await Task.Delay(1000);
            return 5;
        }
        static async Task MethodWithProblems(int firstParam, int secondParam)
        {
            Console.WriteLine("Enter");
            await Task.Run(() =>
            {
                // Вызвать длительно выполняющийся метод.
                Thread.Sleep(2000);
                Console.WriteLine("First Complete");
                // Вызвать еще один длительно выполняющийся метод, который терпит
                // неудачу из-за того, что значение второго параметра выходит
                //за пределы допустимого диапазона.
                Console.WriteLine("Something bad happened");
            });
        }
        static async Task MethodWithProblemsFixed(int firstParam, int secondParam)
        {
            Console.WriteLine("Enter");
            if (secondParam < 0)
            {
                Console.WriteLine("Bad data");
                return;
            }
            await actualImplementation();
            async Task actualImplementation()
            {
                await Task.Run(() =>
                {
                    // Вызвать длительно выполняющийся метод.
                    Thread.Sleep(2_000);
                    Console.WriteLine("First Complete");
                    // Вызвать еще один длительно выполняющийся метод, который терпит
                    // неудачу из-за того, что значение второго параметра выходит
                    // за пределы допустимого диапазона.
                    Console.WriteLine("Something bad happened");
                });
            }
        }
    }
}
