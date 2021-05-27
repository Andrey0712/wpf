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

        public bool CanselAsyngMetod { get ; set ; }//устанавливаем флаг для отмены асинхронного добавления

        public event InsertUserDelegate EventInsertItem ;

        public int Count()//подсчитывае кол-во юзеров в базе
        {
            return _context.Users.Count();
        }
        public void InsertUser(int count, ManualResetEvent mrse)
        {
            using (var transaction = _context.Database.BeginTransaction())//https://metanit.com/sharp/entityframework/3.11.php
            {
                try
                {
                    Stopwatch stopWatch = new Stopwatch();//cколько времени на добовление юзеров
                    stopWatch.Start();
                    for (int i = 0; i < count; i++)
                    {
                        mrse.WaitOne();//приостановка потока
                        if (CanselAsyngMetod)
                        {
                            //CanselAsyngMetod = false;

                            break;// остановка добавления при нажатии отмены
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
                            EventInsertItem(i + 1);// тоже самое ниже
                                                   //EventInsertItem?.Invoke(i + 1);
                        Debug.WriteLine("Insert user " + appUser.Id);
                    }
                    stopWatch.Stop();//остановка подсчета времени на добавление в БД
                                     // Get the elapsed time as a TimeSpan value.
                    TimeSpan ts = stopWatch.Elapsed;//считает тики и переводит в формат время

                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
                    Debug.WriteLine("Затрачено времени на генерацию Юзеров: " + elapsedTime);
                    if (!CanselAsyngMetod)
                    {
                        //  Сохраняем транзакцию если флаг для отмены асинхронного добавления false
                        transaction.Commit();
                    }
                   
                }
                catch (Exception ex)
                {
                     transaction.Rollback();//  Отмена транзакции

                }
                CanselAsyngMetod = false;
            }
        }

        public Task InsertUserAsync(int count, ManualResetEvent mrse)//запуск инсерта асинхронно
        {
            return Task.Run(()=> InsertUser(count, mrse));
        }
    }
}
