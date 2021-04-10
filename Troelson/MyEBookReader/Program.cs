using FizzWare.NBuilder.Dates;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace MyEBookReader
{
    class Program
    {
        static string theEBook;
        static void GetBook()
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += (s, eArgs) =>
            {
                theEBook = eArgs.Result;
                Console.WriteLine(" Download complete. ");
                GetStats();
            };
            // Загрузить электронную книгу Чарльза Диккенса "A Tale of Two Cities".
            // Может понадобиться двукратное выполнение этого кода, если ранее вы
            //не посещали данный сайт, поскольку при первом его посещении появляется
            // окно с сообщением, предотвращающее нормальное выполнение кода.
            wc.DownloadStringAsync(new Uri("http: //www.gutenberg.org/files/98/98-8 . txt"));
        }
        static void GetStats()
        {
            // Получить слова из электронной книги.
            string[] words = theEBook.Split(new char[] { ' ','\u000A', '.', ';', ':', ';', '-', '?', '/' },
            StringSplitOptions.RemoveEmptyEntries); 
            // Найти 10 наиболее часто встречающихся слов.
            string[] tenMostCommon = FindTenMostCommon(words);
            // Получить самое длинное слово.
            string longestWord = FindLongestWord(words);
            // Когда все задачи завершены, построить строку,
            // показывающую всю статистику в окне сообщений.
            StringBuilder bookStats = new StringBuilder("Ten Most Common Words are:\n");
            foreach (string s in tenMostCommon)
            {
                bookStats.AppendLine(s);
            }
            bookStats.AppendFormat("Longest word is: {0} ", longestWord); //Самое длинное слово
            bookStats.AppendLine();
            Console.WriteLine(bookStats.ToString(), "Book info"); // Информация о книге
         }

        //FindLongestWord() находит самое длинное слово:
        private static string[] FindTenMostCommon(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;
            string[] commonWords = (frequencyOrder.Take(10)).ToArray();
            return commonWords;
        }
        private static string FindLongestWord(string[] words)
        {
            return (from w in words
                    orderby w.Length descending
                    select w).FirstOrDefault();
        }
            static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            GetBook();
        }
    }
}

