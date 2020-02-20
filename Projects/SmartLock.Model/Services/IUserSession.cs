﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLock.Model.Response;

namespace SmartLock.Model.Services
{
    public interface IUserSession
    {
        bool IsLoggedIn { get; }
        int UserId { get; }
        string Token { get; }
        string FirstName { get; }
        string LastName { get; }
        string Phone { get; }
        string Email { get; }
        int ResPortraitId { get; }
        string PushRegId { get; }

        void Start(TokenPostResponseDto dto);
        void Start(MePostResponseDto dto);
        void LogOut();
        void SavePushRegId(string pushRegId);
    }
}
