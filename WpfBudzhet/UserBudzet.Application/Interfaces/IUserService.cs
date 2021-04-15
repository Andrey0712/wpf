using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UserBudzet.Application.Interfaces
{
    public delegate void InsertUserDelegate(int i);
    public interface IUserService
    {
        event InsertUserDelegate EventInsertItem;//вызывается при добавлении елемента
        int Count();//подсчет юзеров в базе
        void InsertUser(int count, ManualResetEvent mrse);//+юзеры

        bool CanselAsyngMetod { get; set; }
        Task InsertUserAsync(int count, ManualResetEvent mrse);
    }
}
