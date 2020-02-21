using SmartLock.Model.Ble;
using SmartLock.Model.Request;
using SmartLock.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLock.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IWebService _webService;
        private readonly IUserSession _userSession;

        public UserService(IWebService webService, IUserSession userSession)
        {
            _webService = webService;
            _userSession = userSession;
        }

        public async Task Login(string username, string password)
        {
            var tokenPostResponse = await _webService.Token(new TokenPostDto
            {
                Username = username,
                Password = password
            });

            // Save token
            _userSession.Start(tokenPostResponse);

            var meGetResponse = await _webService.GetMe();

            // Save user info
            _userSession.Start(meGetResponse);
        }

        public void Logout()
        {
            _userSession.LogOut();
        }
    }
}