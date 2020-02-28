using System;
using SmartLock.Model.Response;
using SmartLock.Model.Services;

namespace SmartLock.Logic.Services
{
    public class UserSession : IUserSession
    {
        private const string UserSettingIdentifier = "UserSettings";
        private const string GeneralSettingIdentifier = "GeneralSettings";

        private readonly ISettingsService _settings;

        private SettingsModel _settingsModel;
        private GeneralModel _generalModel;

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

        public bool RememberMe => _generalModel?.RememberMe ?? false;

        public UserSession(ISettingsService settings)
        {
            _settings = settings;

            LoadGeneralSettings();
            LoadUserSettings();
        }

        public void Start(TokenPostResponseDto dto)
        {
            _settingsModel = new SettingsModel
            {
                UserId = dto.UserId,
                Token = dto.Token
            };

            SaveUserSettings();
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

            SaveUserSettings();
        }

        public void Update(string firstName, string lastName, string email, string phone)
        {
            _settingsModel.FirstName = firstName;
            _settingsModel.LastName = lastName;
            _settingsModel.Phone = email;
            _settingsModel.Email = phone;

            SaveUserSettings();
        }

        public void Update(int resPortraitId)
        {
            _settingsModel.ResPortraitId = resPortraitId;

            SaveUserSettings();
        }

        public void LogOut()
        {
            _generalModel = null;
            _settingsModel = null;

            DeleteGeneralSettings();
            DeleteUserSettings();
        }

        public void SavePushRegId(string pushRegId)
        {
            if (_settingsModel == null)
            {
                throw new Exception("The account must have started login");
            }
            _settingsModel.PushRegId = pushRegId;
            SaveUserSettings();
        }

        public void SaveRememberMe(bool rememberMe)
        {
            if (_generalModel == null)
            {
                _generalModel = new GeneralModel();
            }
            _generalModel.RememberMe = rememberMe;

            SaveGeneralSettings();
        }

        public void SaveKeyboxStatus(bool status)
        {
            if (_settingsModel == null)
            {
                throw new Exception("The account must have started login");
            }
            _settingsModel.KeyboxStatus = status;
            SaveUserSettings();
        }

        private void LoadUserSettings()
        {
            _settingsModel = _settings.LoadObject<SettingsModel>(UserSettingIdentifier);
        }

        private void SaveUserSettings()
        {
            _settings.SaveObject(UserSettingIdentifier, _settingsModel);
        }

        private void DeleteUserSettings()
        {
            _settings.DeleteObject(UserSettingIdentifier);
        }

        private void LoadGeneralSettings()
        {
            _generalModel = _settings.LoadObject<GeneralModel>(GeneralSettingIdentifier);
        }

        private void SaveGeneralSettings()
        {
            _settings.SaveObject(GeneralSettingIdentifier, _generalModel);
        }

        private void DeleteGeneralSettings()
        {
            _settings.DeleteObject(GeneralSettingIdentifier);
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

        public class GeneralModel
        {
            public bool RememberMe { get; set; }
        }
    }
}