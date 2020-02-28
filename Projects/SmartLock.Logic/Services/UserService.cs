﻿using SmartLock.Model.Models;
using SmartLock.Model.PushNotification;
using SmartLock.Model.Request;
using SmartLock.Model.Services;
using System.Threading.Tasks;

namespace SmartLock.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IWebService _webService;
        private readonly IUserSession _userSession;
        private readonly ICacheManager _cacheManager;
        private readonly IPushNotificationService _pushNotificationService;

        public UserService(IWebService webService, IUserSession userSession, ICacheManager cacheManager, IPushNotificationService pushNotificationService)
        {
            _webService = webService;
            _userSession = userSession;
            _cacheManager = cacheManager;
            _pushNotificationService = pushNotificationService;
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

        public async Task CreateFeedback(string feedback)
        {
            await _webService.CreateFeedback(new FeedbackPostDto()
            {
                Content = feedback
            });
        }

        public async Task LogOut()
        {
            await _pushNotificationService.UnregisterAsync();

            _userSession.LogOut();
        }

        public async Task<Cache> GetCachedPortrait(int portraitId, bool force = false)
        {
            _cacheManager.Init(CacheManager.PortraitStorageKey);

            Cache cache = null;

            // Read from disk then
            var key = portraitId.ToString();
            cache = _cacheManager.Get(key);

            if (cache == null || force)
            {
                // Read from web api last
                var data = await _webService.GetPortrait(portraitId);
                cache = _cacheManager.Save(data, key);
            }

            return cache;
        }

        public async Task<Cache> GetCachedMyPortrait(bool force = false)
        {
            if (_userSession.ResPortraitId == 0) return null;
            return await GetCachedPortrait(_userSession.ResPortraitId, force);
        }

        public async Task UpdatePortrait(byte[] data)
        {
            var resPortraitId = await _webService.UpdatePortrait(data);

            _userSession.Update(resPortraitId);
        }
    }
}