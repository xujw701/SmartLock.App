using System;
using System.Collections.Generic;
using System.Linq;
using SmartLock.Model;
using SmartLock.Model.Services;

namespace SmartLock.Logic.Environment
{
    public class EnvironmentManager : IEnvironmentManager
    {
#if DEBUG
        private readonly string DefaultEnvironment = "DEV";
#else
        private readonly string DefaultEnvironment = "PRD";
#endif
        private readonly ISettingsService _settings;
        private readonly List<WebApiEnvironment> _environments;

        private WebApiEnvironment _selectedEnvionment;

        public EnvironmentManager(ISettingsService settings)
        {
            _settings = settings;

            var environments = new List<WebApiEnvironment>
            {
                new WebApiEnvironment("DEV", "https://devsmartelockserviceapi.azurewebsites.net"),
                new WebApiEnvironment("PRD", "https://prdsmartelockserviceapi.azurewebsites.net")
            };

#if DEBUG
            environments.Add(new WebApiEnvironment("DEBUG", "http://localhost:52178"));
#endif

            _environments = environments;
            _selectedEnvionment = _environments.FirstOrDefault(e => e.Name == _settings.LoadString("Environment")) ?? _environments.FirstOrDefault(e => e.Name == DefaultEnvironment) ?? _environments[0];
        }

        public List<WebApiEnvironment> Environments => _environments;

        public WebApiEnvironment SelectedEnvironment
        {
            get => _selectedEnvionment;
            set
            {
                _selectedEnvionment = value;
                _settings.Save("Environment", SelectedEnvironment.Name);
            }
        }

        public Uri FormatUriForSelectedEnvironment(string action, params object[] methodParameters)
        {
            return new Uri(string.Format(GetUri(action), methodParameters));
        }

        private string GetUri(string action)
        {
            var apiBase = SelectedEnvironment.UriFormat;

            if (apiBase.EndsWith("/"))
            {
                apiBase = apiBase.Substring(0, apiBase.Length - 1);
            }

            if (string.IsNullOrEmpty(action))
            {
                return apiBase + "/" + "{0}";
            }

            if (!action.StartsWith("/"))
            {
                action = "/" + action;
            }

            return apiBase + action + "/" + "{0}";
        }
    }
}