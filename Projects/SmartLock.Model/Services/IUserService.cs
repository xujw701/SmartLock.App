using SmartLock.Model.Ble;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface IUserService
    {
        Task Login(string username, string password);
        Task UpdateMe(string firstName, string lastName, string email, string phone);
        Task<bool> UpdatePassword(string oldPassword, string password);
        Task CreateFeedback(string feedback);
        Task LogOut();
    }
}
