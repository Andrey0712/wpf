using System;
using System.Collections.Generic;
using System.Text;

namespace UserBudzet.Application.Interfaces
{
    public delegate void InsertUserDelegate(int i);
    public interface IUserService
    {
        event InsertUserDelegate EventInsertItem;//вызывается при добавлении елемента
        int Count();//подсчет юзеров в базе
        void InsertUser(int count);//+юзеры
    }
}
