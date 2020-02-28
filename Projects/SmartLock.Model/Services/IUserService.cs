using SmartLock.Model.Ble;
using SmartLock.Model.Models;
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
        Task<Cache> GetCachedPortrait(int portraitId, bool force = false);
        Task<Cache> GetCachedMyPortrait(bool force = false);
        Task UpdatePortrait(byte[] data);
    }
}
