using SmartLock.Infrastructure;
using SmartLock.Model.PushNotification;
using SmartLock.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Logic.PushNotification
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IWebService _webService;
        private readonly IUserSession _userSession;

        private event Action<string> DeviceTokenObtained;

        private static string DeviceToken;
        private static bool RegisterWhenTokenObtained;

        public PushNotificationService(IWebService webService, IUserSession userSession)
        {
            _webService = webService;
            _userSession = userSession;

            RegisterWhenTokenObtained = false;
        }

        public void BindDeviceTokenListener()
        {
            DeviceTokenObtained += OnDeviceTokenObtained;
            IoC.Resolve<IDevicePushNotifications>().BindDeviceTokenListener(DeviceTokenObtained);
        }

        public void OnDeviceTokenObtained(string deviceToken)
        {
            DeviceToken = deviceToken;

            if (RegisterWhenTokenObtained)
            {
                Register();
            }
        }

        public void Register()
        {
            Task.Run(async () =>
            {
                await RegisterAsync();
            });
        }

        public void Unregister()
        {
            Task.Run(async () =>
            {
                await UnregisterAsync();
            });
        }

        public void Reregiser()
        {
            Task.Run(async () =>
            {
                await UnregisterAsync();
                await RegisterAsync();
            });
        }

        private async Task RegisterAsync()
        {
            var deviceRegistration = IoC.Resolve<IDevicePushNotifications>().GetDeviceRegistration();

            // Android doesn't need to wait the device token being obtained
            if (!string.IsNullOrEmpty(deviceRegistration.Handle))
            {
                DeviceToken = deviceRegistration.Handle;
            }

            // Device token has been obtained
            if (!string.IsNullOrEmpty(DeviceToken))
            {
                var registrationId = await RequestNotificationRegistrationId(DeviceToken);

                var tags = new List<string>() { _userSession.UserId.ToString(), "global" };

                deviceRegistration.Handle = DeviceToken;
                deviceRegistration.Tags = tags.ToArray();

                await UpdateRegistration(registrationId, deviceRegistration);
            }
            else
            {
                RegisterWhenTokenObtained = true;
            }
        }

        public async Task UnregisterAsync()
        {
            if (!string.IsNullOrEmpty(_userSession.PushRegId))
            {
                await _webService.RemoveRegistration(_userSession.PushRegId);

                _userSession.SavePushRegId(null);
            }
        }

        private async Task<HttpStatusCode> UpdateRegistration(string registrationId, DeviceRegistration deviceRegistration)
        {
            try
            {
                await _webService.UpsertRegistration(registrationId, deviceRegistration);
            }
            catch (HttpRequestException httpException)
            {
                if (httpException.Message.Contains(HttpStatusCode.Gone.ToString()))
                {
                    return HttpStatusCode.Gone;
                }
                return HttpStatusCode.InternalServerError;
            }
            return HttpStatusCode.OK;
        }

        private async Task<string> RequestNotificationRegistrationId(string handle)
        {
            //if (_userSession.PushRegId != null)
            //{
            //    return _userSession.PushRegId;
            //}

            var notificationHandle = new NotificationHandle()
            {
                Handle = handle
            };

            var registrationId = await _webService.RequestNotificationRegistrationId(notificationHandle);

            _userSession.SavePushRegId(registrationId);

            return registrationId;
        }
    }
}
