using SmartLock.Model.Ble;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface IUserService
    {
        Task Login(string username, string password);
        void Logout();
    }
}
