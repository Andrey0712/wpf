using Budzet.Domain;
using Budzet.EFData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserBudzet.Application.Interfaces;

namespace Budzet.Infrastructure.Services
{
       public class UserService : IUserService

    { 
        private readonly EFDataContext _context;
        public UserService()
        {
            _context = new EFDataContext();
        }

        public bool CanselAsyngMetod { get ; set ; }

        public event InsertUserDelegate EventInsertItem ;

        public int Count()//
        {
            return _context.Users.Count();
        }
        public void InsertUser(int count, ManualResetEvent mrse)
        {
            Stopwatch stopWatch = new Stopwatch();//cколько времени на добовление юзеров
            stopWatch.Start();
            for (int i = 0; i < count; i++)
            {
                mrse.WaitOne();//задержка
                if (CanselAsyngMetod)
                {
                    CanselAsyngMetod = false;
                    break;
                }
                AppUser appUser = new AppUser
                {
                    Name = "Name" + i,
                    DebitKredit = true,
                    Tranіaction = DateTime.Now.AddDays(i),
                    Details = "qqqq",
                    Image = "Fotka"
                };
                _context.Users.Add(appUser);
                _context.SaveChanges();
                if (EventInsertItem != null)
                    EventInsertItem(i + 1);// тоже самое в 45 стр
                //EventInsertItem?.Invoke(i + 1);
                Console.WriteLine("Insert user "+ appUser.Id);
            }
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;//считает тики и переводит в формат время

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("Затрачено времени на генерацию Юзеров: " + elapsedTime);
        }

        public Task InsertUserAsync(int count, ManualResetEvent mrse)
        {
            return Task.Run(()=> InsertUser(count, mrse));
        }
    }
}
