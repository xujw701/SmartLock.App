using SmartLock.Model.Request;
using SmartLock.Model.Services;
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

        public async Task UpdateMe(string firstName, string lastName, string email, string phone)
        {
            await _webService.UpdateMe(new MePutDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone
            });

            // Save token
            //_userSession.Update(firstName, lastName, email, phone);

            var meGetResponse = await _webService.GetMe();

            // Save user info
            _userSession.Start(meGetResponse);
        }

        public async Task<bool> UpdatePassword(string oldPassword, string password)
        {
            var result = await _webService.Auth(new TokenPostDto
            {
                Username = _userSession.UserName,
                Password = oldPassword
            });

            if (result)
            {
                await _webService.UpdateMe(new MePutDto
                {
                    FirstName = _userSession.FirstName,
                    LastName = _userSession.LastName,
                    Email = _userSession.Email,
                    Phone = _userSession.Phone,
                    Password = password
                });

                var meGetResponse = await _webService.GetMe();

                // Save user info
                _userSession.Start(meGetResponse);

                return true;
            }
            return false;
        }

        public void Logout()
        {
            _userSession.LogOut();
        }
    }
}