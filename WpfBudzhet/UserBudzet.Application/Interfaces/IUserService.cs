using System;
using System.Collections.Generic;
using System.Text;

namespace UserBudzet.Application.Interfaces
{
    
    public interface IUserService
    {
        int Count();//подсчет юзеров в базе
        void InsertUser(int count);//+юзеры
    }
}
