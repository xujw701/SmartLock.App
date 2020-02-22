using System;
using SmartLock.Model.Response;
using SmartLock.Model.Services;

namespace SmartLock.Logic.Services
{
    public class UserSession : IUserSession
    {
        private const string ObjectIdentifier = nameof(UserSession);

        private readonly ISettingsService _settings;

        private SettingsModel _settingsModel;

        public bool IsLoggedIn => _settingsModel != null && _settingsModel.UserId > 0;
        public int UserId => _settingsModel?.UserId ?? throw new Exception("Login process must be started to retrieve user id");
        public string UserName => _settingsModel?.UserName ?? throw new Exception("Login process must be started to retrieve token");
        public string Token => _settingsModel?.Token ?? throw new Exception("Login process must be started to retrieve token");
        public string FirstName => _settingsModel?.FirstName ?? throw new Exception("Login process must be started to retrieve name");
        public string LastName => _settingsModel?.LastName ?? throw new Exception("Login process must be started to retrieve name");
        public string Phone => _settingsModel?.Phone ?? throw new Exception("Login process must be started to retrieve phone");
        public string Email => _settingsModel?.Email ?? throw new Exception("Login process must be started to retrieve email");
        public int ResPortraitId => _settingsModel?.ResPortraitId ?? 0;
        public string PushRegId => _settingsModel?.PushRegId;
        public bool KeyboxStatus => _settingsModel != null &&  _settingsModel.KeyboxStatus;

        public UserSession(ISettingsService settings)
        {
            _settings = settings;

            LoadObject();
        }

        public void Start(TokenPostResponseDto dto)
        {
            if (IsLoggedIn)
            {
                throw new Exception("The account has already logged in");
            }

            _settingsModel = new SettingsModel
            {
                UserId = dto.UserId,
                Token = dto.Token
            };

            SaveObject();
        }

        public void Start(MePostResponseDto dto)
        {
            _settingsModel.CompanyId = dto.CompanyId;
            _settingsModel.BranchId = dto.BranchId;
            _settingsModel.UserName = dto.UserName;
            _settingsModel.FirstName = dto.FirstName;
            _settingsModel.LastName = dto.LastName;
            _settingsModel.Phone = dto.Phone;
            _settingsModel.Email = dto.Email;
            _settingsModel.ResPortraitId = dto.ResPortraitId;

            SaveObject();
        }

        public void Update(string firstName, string lastName, string email, string phone)
        {
            _settingsModel.FirstName = firstName;
            _settingsModel.LastName = lastName;
            _settingsModel.Phone = email;
            _settingsModel.Email = phone;

            SaveObject();
        }

        public void LogOut()
        {
            _settingsModel = null;
            DeleteObject();
        }

        public void SavePushRegId(string pushRegId)
        {
            if (_settingsModel == null)
            {
                throw new Exception("The account must have started login");
            }
            _settingsModel.PushRegId = pushRegId;
            SaveObject();
        }

        public void SaveKeyboxStatus(bool status)
        {
            if (_settingsModel == null)
            {
                throw new Exception("The account must have started login");
            }
            _settingsModel.KeyboxStatus = status;
            SaveObject();
        }

        private void LoadObject()
        {
            _settingsModel = _settings.LoadObject<SettingsModel>(ObjectIdentifier);
        }

        private void SaveObject()
        {
            _settings.SaveObject(ObjectIdentifier, _settingsModel);
        }

        private void DeleteObject()
        {
            _settings.DeleteObject(ObjectIdentifier);
        }

        public class SettingsModel
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Token { get; set; }
            public int CompanyId { get; set; }
            public int BranchId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int? ResPortraitId { get; set; }
            public string PushRegId { get; set; }

            public bool KeyboxStatus { get; set; }
        }
    }
}